using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicManager.Domain.Entities
{
    internal class AlbumGenre
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public DateTime Created { get; set; }
    }
}
