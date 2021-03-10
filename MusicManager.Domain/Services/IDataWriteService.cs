using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataWriteService
    {
        Task<bool> DeleteArtist(int id);
    }
}
