using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
