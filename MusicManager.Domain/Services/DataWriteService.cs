using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public class DataWriteService : IDataWriteService
    {
        private readonly MusicManagerContext _dbContext;

        public DataWriteService(MusicManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateArtist(ArtistEditDto dto)
        {
            var newArtist = new Artist()
            {
                Name = dto.Name,
                Created = DateTime.UtcNow
            };

            _dbContext.Artists.Add(newArtist);
            await _dbContext.SaveChangesAsync();

            return newArtist.Id;
        }

        public async Task<bool> UpdateArtist(int id, ArtistEditDto dto)
        {
            var artist = await _dbContext.Artists.FindAsync(id);

            if (artist == null)
                return false;

            artist.Name = dto.Name;
            artist.Updated = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteArtist(int id)
        {
            var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
                return false;

            _dbContext.Artists.Remove(artist);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> CreateAlbum(AlbumEditDto dto)
        {
            var now = DateTime.UtcNow;
            var newAlbum = new Album()
            {
                Title = dto.Title,
                ReleaseYear = dto.ReleaseYear,
                ArtistId = dto.ArtistId,
                AlbumGenres = dto.GenreIds.Select(g => new AlbumGenre { GenreId = g, Created = now }).ToList(),
                Created = now
            };

            _dbContext.Albums.Add(newAlbum);
            await _dbContext.SaveChangesAsync();

            return newAlbum.Id;
        }

        public async Task<bool> UpdateAlbum(int id, AlbumEditDto dto)
        {
            var album = await _dbContext.Albums.Include(a => a.AlbumGenres)
                                                .SingleOrDefaultAsync(a => a.Id == id);

            if (album == null)
                return false;

            var now = DateTime.UtcNow;
            album.Title = dto.Title;
            album.ReleaseYear = dto.ReleaseYear;
            album.ArtistId = dto.ArtistId;
            album.Updated = now;

            var removedGenres = album.AlbumGenres.Where(g => !dto.GenreIds.Any(dtog => dtog == g.GenreId)).ToArray();
            foreach (var removedGenre in removedGenres)
            {
                album.AlbumGenres.Remove(removedGenre);
            }
            
            var addedGenres = dto.GenreIds.Where(dtog => !album.AlbumGenres.Any(g => g.GenreId == dtog)).ToArray();
            foreach (var addedGenre in addedGenres)
            {
                album.AlbumGenres.Add(new AlbumGenre { GenreId = addedGenre, Created = now });
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAlbum(int id)
        {
            var album = await _dbContext.Albums.FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
                return false;

            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
