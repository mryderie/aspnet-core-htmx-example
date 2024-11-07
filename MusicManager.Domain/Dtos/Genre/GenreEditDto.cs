using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos.Genre
{
    public class GenreEditDto : BaseEditDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
