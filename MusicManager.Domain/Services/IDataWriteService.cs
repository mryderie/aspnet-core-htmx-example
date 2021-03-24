using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataWriteService
    {
        Task<int> CreateArtist(ArtistEditDto dto);
        Task<bool> UpdateArtist(int id, ArtistEditDto dto);
        Task<bool> DeleteArtist(int id);
        
        Task<int> CreateAlbum(AlbumEditDto dto);
        Task<bool> UpdateAlbum(int id, AlbumEditDto dto);
        Task<bool> DeleteAlbum(int id);
    }
}
