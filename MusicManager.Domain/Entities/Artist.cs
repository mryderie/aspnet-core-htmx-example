using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicManager.Domain.Entities
{
    internal class Artist : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
