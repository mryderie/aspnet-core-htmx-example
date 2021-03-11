using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.Domain.Dtos.Album
{
    public class AlbumDto : BaseViewDto
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
