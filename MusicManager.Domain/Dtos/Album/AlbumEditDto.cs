using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos.Album
{
    public class AlbumEditDto : BaseEditDto
    {

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [DisplayName("Release Year")]
        public int ReleaseYear { get; set; }

        [Required]
        [DisplayName("Artist")]
        public int ArtistId { get; set; }

        [DisplayName("Genres")]
        public IList<int> GenreIds { get; set; } = new List<int>();
    }
}
