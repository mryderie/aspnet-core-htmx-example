using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.Domain.Dtos
{
    public class AlbumDto : BaseDto
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
