using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicManager.Domain.Dtos.Artist
{
    public class ArtistViewDto: BaseViewDto
    {
        public string Name { get; set; }
        
        [DisplayName("Album Count")]
        public int AlbumCount { get; set; }
    }
}
