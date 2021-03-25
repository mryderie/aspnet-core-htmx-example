using System.ComponentModel;

namespace MusicManager.Domain.Dtos.Genre
{
    public class GenreViewDto : BaseViewDto
    {
        public string Name { get; set; }

        [DisplayName("Albums")]
        public int AlbumCount { get; set; }
    }
}
