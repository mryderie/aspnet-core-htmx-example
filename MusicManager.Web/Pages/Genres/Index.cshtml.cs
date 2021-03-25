using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicManager.Domain.Dtos.Genre;
using MusicManager.Domain.Services;
using MusicManager.Web.Helpers;

namespace MusicManager.Web.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private const int PAGE_SIZE = 10;
        private readonly IDataReadService _dataReadService;
        private readonly IDataWriteService _dataWriteService;

        private enum Sort
        {
            NameAsc,
            NameDesc,
            AlbumCountAsc,
            AlbumCountDesc,
            CreatedAsc,
            CreatedDesc,
            UpdatedAsc,
            UpdatedDesc
        }

        private Dictionary<Sort, string> _sorts = new Dictionary<Sort, string>
        {
            { Sort.NameAsc, "name_asc" },
            { Sort.NameDesc, "name_desc" },
            { Sort.AlbumCountAsc, "albumCount_asc" },
            { Sort.AlbumCountDesc, "albumCount_desc" },
            { Sort.CreatedAsc, "created_asc" },
            { Sort.CreatedDesc, "created_desc" },
            { Sort.UpdatedAsc, "updated_asc" },
            { Sort.UpdatedDesc, "updated_desc" },
        };

        // These Sort properties control the URL parameters for the sorting links in the table header
        public string NameSort { get; set; }
        public string AlbumCountSort { get; set; }
        public string CreatedSort { get; set; }
        public string UpdatedSort { get; set; }
        public PaginatedList<GenreViewDto> Genres { get; set; }

        public IndexModel(IDataReadService dataReadService, IDataWriteService dataWriteService)
        {
            _dataReadService = dataReadService;
            _dataWriteService = dataWriteService;
        }

        public async Task OnGetAsync(string sortOrder, string currentFilter,
                                    string searchString, int? pageIndex)
        {
            // name_asc is the default sort value, so an unset or invalid parameter value will be replaced with that value
            var currentSort = _sorts.ContainsValue(sortOrder) ? sortOrder : _sorts[Sort.NameAsc];
            NameSort = currentSort == _sorts[Sort.NameAsc] ? _sorts[Sort.NameDesc] : _sorts[Sort.NameAsc];
            AlbumCountSort = currentSort == _sorts[Sort.AlbumCountAsc] ? _sorts[Sort.AlbumCountDesc] : _sorts[Sort.AlbumCountAsc];
            CreatedSort = currentSort == _sorts[Sort.CreatedAsc] ? _sorts[Sort.CreatedDesc] : _sorts[Sort.CreatedAsc];
            UpdatedSort = currentSort == _sorts[Sort.UpdatedAsc] ? _sorts[Sort.UpdatedDesc] : _sorts[Sort.UpdatedAsc];

            // For the initial search, searchString parameter is set. For subsequent refreshes/pages, currentFilter is set.
            if (searchString != null)
            {
                pageIndex = 1;
                currentFilter = searchString;
            }

            string sortField = null;
            bool sortDesc = false;

            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                var splitSort = sortOrder.Split('_');
                if (splitSort.Length == 2)
                {
                    sortField = splitSort[0];
                    sortDesc = splitSort[1] == "desc";
                }
            }

            var result = await _dataReadService.GetGenresPage(currentFilter, sortField, sortDesc, pageIndex ?? 1, PAGE_SIZE);

            var parameters = new Dictionary<string, string> {
                { ParamName.PageIndex, (pageIndex ?? 1).ToString() },
                { ParamName.SortOrder, currentSort },
                { ParamName.CurrentFilter, currentFilter }
            };

            Genres = new PaginatedList<GenreViewDto>(result.pageItems, result.totalCount, PAGE_SIZE, pageIndex ?? 1, parameters);
        }

        public async Task<IActionResult> OnGetDetailsModal(int id)
        {
            var genre = await _dataReadService.GetGenreView(id);
            if (genre == null)
            {
                return NotFound();
            }

            return Partial("_DetailsModal", genre);
        }

        public async Task<IActionResult> OnGetEditModal(int? id)
        {
            // Create item form
            if (!id.HasValue)
                return Partial("_EditModal");

            // Update item form
            var genre = await _dataReadService.GetGenreEdit(id.Value);
            if (genre == null)
            {
                return NotFound();
            }

            return Partial("_EditModal", genre);
        }

        public async Task<IActionResult> OnPostAsync(GenreEditDto model)
        {
            //temp - to test handling a server-side validation error
            if (model.Name == "error")
                ModelState.AddModelError("Name", "Server-side validation error...");

            if (ModelState.IsValid)
            {
                await _dataWriteService.CreateGenre(model);

                Response.Headers.Add("HX-Trigger", "gridItemEdit");
                return new NoContentResult();
            }

            return Partial("_EditModal", model);
        }

        public async Task<IActionResult> OnPutAsync(int id, GenreEditDto model)
        {
            //temp - to test handling a server-side validation error
            if (model.Name == "error")
                ModelState.AddModelError("Name", "Server-side validation error...");

            if (ModelState.IsValid)
            {
                var result = await _dataWriteService.UpdateGenre(id, model);

                if (!result)
                    return NotFound();

                Response.Headers.Add("HX-Trigger", "gridItemEdit");
                return new NoContentResult();
            }

            return Partial("_EditModal", model);
        }

        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            var result = await _dataWriteService.DeleteGenre(id);

            if (!result)
                return NotFound();

            Response.Headers.Add("HX-Trigger", "gridItemDelete");
            return new NoContentResult();
        }
    }
}
