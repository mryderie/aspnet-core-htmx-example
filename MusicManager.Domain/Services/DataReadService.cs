using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public class DataReadService : IDataReadService
    {
        private readonly MusicManagerContext _dbContext;

        public DataReadService(MusicManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ArtistDto> GetArtist(int id)
        {
            return await _dbContext.Artists
                                    .Select(a => new ArtistDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.Albums.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IList<ArtistDto> pageItems, int totalCount)> GetArtistsPage(string search, string sortField,
                                                                                    bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Artists.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => EF.Functions.Like(a.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "albumcount":
                    if (descending)
                        query = query.OrderByDescending(a => a.Albums.Count);
                    else
                        query = query.OrderBy(a => a.Albums.Count);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(a => a.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(a => a.Updated);
                    else
                        query = query.OrderBy(a => a.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(a => a.Name);
                    else
                        query = query.OrderBy(a => a.Name);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(a => new ArtistDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.Albums.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }
    }
}
