using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.Domain.Dtos.Artist
{
    public class ArtistViewDto: BaseViewDto
    {
        public string Name { get; set; }
        public int AlbumCount { get; set; }
    }
}
