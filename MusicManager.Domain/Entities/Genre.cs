using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicManager.Domain.Entities
{
    internal class Genre : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public ICollection<AlbumGenre> AlbumGenres { get; set; }
    }
}
