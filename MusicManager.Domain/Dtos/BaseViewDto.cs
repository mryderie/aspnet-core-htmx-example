using System;
using System.ComponentModel.DataAnnotations;

namespace MusicManager.Domain.Dtos
{
    public class BaseViewDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public DateTime? Updated { get; set; }
    }
}