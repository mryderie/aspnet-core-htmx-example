using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Dtos.Genre;
using MusicManager.Domain.Dtos.Track;
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
            var artist = await _dbContext.Artists.FindAsync(id);

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
            var album = await _dbContext.Albums.FindAsync(id);

            if (album == null)
                return false;

            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> CreateGenre(GenreEditDto dto)
        {
            var newGenre = new Genre()
            {
                Name = dto.Name,
                Created = DateTime.UtcNow
            };

            _dbContext.Genres.Add(newGenre);
            await _dbContext.SaveChangesAsync();

            return newGenre.Id;
        }

        public async Task<bool> UpdateGenre(int id, GenreEditDto dto)
        {
            var genre = await _dbContext.Genres.FindAsync(id);

            if (genre == null)
                return false;

            genre.Name = dto.Name;
            genre.Updated = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteGenre(int id)
        {
            var genre = await _dbContext.Genres.FindAsync(id);

            if (genre == null)
                return false;

            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> CreateTrack(TrackEditDto dto)
        {
            var now = DateTime.UtcNow;
            var newTrack = new Track()
            {
                Title = dto.Title,
                TrackNumber = dto.TrackNumber,
                AlbumId = dto.AlbumId,
                Created = now
            };

            _dbContext.Tracks.Add(newTrack);
            await _dbContext.SaveChangesAsync();

            return newTrack.Id;
        }

        public async Task<bool> UpdateTrack(int id, TrackEditDto dto)
        {
            var track = await _dbContext.Tracks.FindAsync(id);

            if (track == null)
                return false;

            track.Title = dto.Title;
            track.TrackNumber = dto.TrackNumber;
            track.AlbumId = dto.AlbumId;
            track.Updated = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTrack(int id)
        {
            var track = await _dbContext.Tracks.FindAsync(id);

            if (track == null)
                return false;

            _dbContext.Tracks.Remove(track);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
