using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicManager.Domain.Entities
{
    internal class Album : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public int ReleaseYear { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public ICollection<AlbumGenre> AlbumGenres { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
