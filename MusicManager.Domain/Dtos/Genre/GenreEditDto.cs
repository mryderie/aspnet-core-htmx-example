using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos.Genre
{
    public class GenreEditDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
