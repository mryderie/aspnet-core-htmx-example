using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
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
