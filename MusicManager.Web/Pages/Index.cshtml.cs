using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicManager.Domain.Services;

namespace MusicManager.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDataReadService _dataReadService;

        public int GenreCount { get; set; }
        public int ArtistCount { get; set; }
        public int AlbumCount { get; set; }
        public int TrackCount { get; set; }

        public IndexModel(IDataReadService dataReadService)
        {
            _dataReadService = dataReadService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var entityCounts = await _dataReadService.GetEntityCounts();

            GenreCount = entityCounts.genreCount;
            ArtistCount = entityCounts.artistCount;
            AlbumCount = entityCounts.albumCount;
            TrackCount = entityCounts.trackCount;

            return Page();
        }
    }
}