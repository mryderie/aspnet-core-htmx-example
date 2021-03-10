using MusicManager.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataReadService
    {
        Task<ArtistDto> GetArtist(int id);
        Task<(IList<ArtistDto> pageItems, int totalCount)> GetArtistsPage(string search, string sortField, bool descending, int pageIndex, int pageSize);
    }
}
