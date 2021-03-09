using System;

namespace MusicManager.Domain.Dtos
{
    public class BaseDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}