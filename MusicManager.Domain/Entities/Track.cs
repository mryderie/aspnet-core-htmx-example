using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicManager.Domain.Entities
{
    internal class Track : BaseEntity
    {
        [Required]
        public int TrackNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public int AlbumId { get; set; }
        
        public Album Album { get; set; }
    }
}
