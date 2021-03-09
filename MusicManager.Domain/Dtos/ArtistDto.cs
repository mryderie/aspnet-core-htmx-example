using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.Domain.Dtos
{
    public class ArtistDto: BaseDto
    {
        public string Name { get; set; }
        public int AlbumCount { get; set; }
    }
}
