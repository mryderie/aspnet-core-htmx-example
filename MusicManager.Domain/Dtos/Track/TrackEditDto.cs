using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos.Track
{
    public class TrackEditDto : BaseEditDto
    {
        [Required]
        [DisplayName("Album")]
        public int AlbumId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [DisplayName("Track Number")]
        public int TrackNumber { get; set; }
    }
}
