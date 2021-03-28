using System.Collections.Generic;
using System.ComponentModel;

namespace MusicManager.Domain.Dtos.Track
{
    public class TrackViewDto : BaseViewDto
    {
        [DisplayName("Artist")]
        public string ArtistName { get; set; }

        [DisplayName("Album")]
        public string AlbumTitle { get; set; }
        
        public string Title { get; set; }

        [DisplayName("Track Number")]
        public int TrackNumber { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
    }
}
