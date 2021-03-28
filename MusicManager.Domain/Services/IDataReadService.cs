using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Dtos.Genre;
using MusicManager.Domain.Dtos.Track;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataReadService
    {
        Task<(int genreCount, int artistCount, int albumCount, int trackCount)> GetEntityCounts();

        Task<ArtistViewDto> GetArtistView(int id);
        Task<(IList<ArtistViewDto> pageItems, int totalCount)> GetArtistsPage(string search, string sortField, bool descending, int pageIndex, int pageSize);

        Task<ArtistEditDto> GetArtistEdit(int id);

        Task<IList<(int artistId, string artistName)>> GetArtistNames();


        Task<AlbumViewDto> GetAlbumView(int id);
        Task<(IList<AlbumViewDto> pageItems, int totalCount)> GetAlbumsPage(int? artistId, string search, string sortField,
                                                                            bool descending, int pageIndex, int pageSize);
        Task<AlbumEditDto> GetAlbumEdit(int id);
        Task<IList<(int albumId, string albumTitle)>> GetAlbumTitles();


        Task<IList<(int genreId, string genreName)>> GetGenreNames();
        Task<GenreViewDto> GetGenreView(int id);
        Task<(IList<GenreViewDto> pageItems, int totalCount)> GetGenresPage(string search, string sortField, bool descending,
                                                                            int pageIndex, int pageSize);
        Task<GenreEditDto> GetGenreEdit(int value);


        Task<TrackViewDto> GetTrackView(int id);
        Task<(IList<TrackViewDto> pageItems, int totalCount)> GetTracksPage(int? albumId, string search, string sortField, bool descending, int pageIndex, int pageSize);
        Task<TrackEditDto> GetTrackEdit(int id);
    }
}
