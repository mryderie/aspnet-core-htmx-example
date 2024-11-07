using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos.Artist
{
    public class ArtistEditDto : BaseEditDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
