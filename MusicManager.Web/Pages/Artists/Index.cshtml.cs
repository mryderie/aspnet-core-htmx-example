using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicManager.Domain.Dtos;
using MusicManager.Domain.Services;
using MusicManager.Web.Helpers;

namespace MusicManager.Web.Pages.Artists
{
    public class IndexModel : PageModel
    {
        private const int PAGE_SIZE = 10;
        private readonly IDataReadService _dataReadService;

        // These Sort properties control the URL parameters for the sorting links in the table header
        public string NameSort { get; set; }
        public string AlbumCountSort { get; set; }
        public string CreatedSort { get; set; }
        public string UpdatedSort { get; set; }
        public PaginatedList<ArtistDto> Artists { get; set; }

        public IndexModel(IDataReadService dataReadService)
        {
            _dataReadService = dataReadService;
        }

        public async Task OnGetAsync(string sortOrder, string currentFilter,
                                    string searchString, int? pageIndex)
        {
            // Sorting by Name asc is the default, so no parameter is needed in the URL from this case
            // todo - fix these magic strings!
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "name_asc";
            AlbumCountSort = sortOrder == "albumCount_asc" ? "albumCount_desc" : "albumCount_asc";
            CreatedSort = sortOrder == "created_asc" ? "created_desc" : "created_asc";
            UpdatedSort = sortOrder == "updated_asc" ? "updated_desc" : "updated_asc";

            // For the initial search, searchString parameter is set. For subsequent refreshes/pages, currentFilter is set.
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            string sortField = null;
            bool sortDesc = false;

            if(!string.IsNullOrWhiteSpace(sortOrder))
            {
                var splitSort = sortOrder.Split('_');
                if(splitSort.Length == 2)
                {
                    sortField = splitSort[0];
                    sortDesc = splitSort[1] == "desc";
                }
            }

            var result = await _dataReadService.GetArtistsPage(searchString, sortField, sortDesc, pageIndex ?? 1, PAGE_SIZE);
            Artists = new PaginatedList<ArtistDto>(result.pageItems, result.totalCount, pageIndex ?? 1, PAGE_SIZE, sortField, searchString);
        }

        public async Task<IActionResult> OnGetDetailsModal(int id)
        {
            var artist = await _dataReadService.GetArtist(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Partial("_DetailsModal", artist);
        }
    }
}
