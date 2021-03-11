using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos.Artist
{
    public class ArtistEditDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
