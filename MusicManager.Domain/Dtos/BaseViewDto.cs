using System;

namespace MusicManager.Domain.Dtos
{
    public class BaseViewDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}