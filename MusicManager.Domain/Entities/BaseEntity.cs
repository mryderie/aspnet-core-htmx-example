using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.Domain.Entities
{
    internal abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
