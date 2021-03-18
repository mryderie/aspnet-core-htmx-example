using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataReadService
    {
        Task<ArtistViewDto> GetArtistView(int id);
        Task<(IList<ArtistViewDto> pageItems, int totalCount)> GetArtistsPage(string search, string sortField, bool descending, int pageIndex, int pageSize);

        Task<ArtistEditDto> GetArtistEdit(int id);


        Task<AlbumViewDto> GetAlbumView(int id);
        Task<(IList<AlbumViewDto> pageItems, ArtistViewDto artist, int totalCount)> GetAlbumsPage(int? artistId, string search, string sortField,
                                                                                                    bool descending, int pageIndex, int pageSize);

        //Task<AlbumEditDto> GetAlbumEdit(int id);
    }
}
