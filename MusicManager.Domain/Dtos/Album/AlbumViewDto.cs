using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MusicManager.Domain.Dtos.Album
{
    public class AlbumViewDto : BaseViewDto
    {
        public string Title { get; set; }
        
        [DisplayName("Tracks")]
        public int TrackCount { get; set; }

        [DisplayName("Release Year")]
        public int ReleaseYear { get; set; }
        public int ArtistId { get; set; }

        [DisplayName("Artist")]
        public string ArtistName { get; set; }

        public IList<string> Genres { get; set; }
    }
}
