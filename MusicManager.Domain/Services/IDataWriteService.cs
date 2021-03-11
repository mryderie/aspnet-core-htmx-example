using MusicManager.Domain.Dtos.Artist;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataWriteService
    {
        Task<int> CreateArtist(ArtistEditDto dto);
        Task<bool> UpdateArtist(int id, ArtistEditDto dto);
        Task<bool> DeleteArtist(int id);
    }
}
