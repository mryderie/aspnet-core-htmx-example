using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicManager.Domain.Dtos.Track;
using MusicManager.Domain.Services;
using MusicManager.Web.Helpers;

namespace MusicManager.Web.Pages.Tracks
{
    public class IndexModel : PageModel
    {
        private const int PAGE_SIZE = 10;
        private readonly IDataReadService _dataReadService;
        private readonly IDataWriteService _dataWriteService;

        private enum Sort
        {
            ArtistNameAsc,
            ArtistNameDesc,
            AlbumTitleAsc,
            AlbumTitleDesc,
            TitleAsc,
            TitleDesc,
            TrackNumberAsc,
            TrackNumberDesc,
            CreatedAsc,
            CreatedDesc,
            UpdatedAsc,
            UpdatedDesc
        }

        private Dictionary<Sort, string> _sorts = new Dictionary<Sort, string>
        {
            { Sort.ArtistNameAsc, "artistName_asc" },
            { Sort.ArtistNameDesc, "artistName_desc" },
            { Sort.AlbumTitleAsc, "albumTitle_asc" },
            { Sort.AlbumTitleDesc, "albumTitle_desc" },
            { Sort.TitleAsc, "title_asc" },
            { Sort.TitleDesc, "title_desc" },
            { Sort.TrackNumberAsc, "trackNumber_asc" },
            { Sort.TrackNumberDesc, "trackNumber_desc" },
            { Sort.CreatedAsc, "created_asc" },
            { Sort.CreatedDesc, "created_desc" },
            { Sort.UpdatedAsc, "updated_asc" },
            { Sort.UpdatedDesc, "updated_desc" },
        };

        // These Sort properties control the URL parameters for the sorting links in the table header
        public string ArtistNameSort { get; set; }
        public string AlbumNameSort { get; set; }
        public string TitleSort { get; set; }
        public string TrackNumberSort { get; set; }
        public string CreatedSort { get; set; }
        public string UpdatedSort { get; set; }

        public PaginatedList<TrackViewDto> Tracks { get; set; }
        public List<SelectListItem> AlbumList { get; set; }

        public IndexModel(IDataReadService dataReadService, IDataWriteService dataWriteService)
        {
            _dataReadService = dataReadService;
            _dataWriteService = dataWriteService;
        }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter,
                                                    string searchString, int? albumId, int? pageIndex)
        {
            // title_asc is the default sort value, so an unset or invalid parameter value will be replaced with that value
            var currentSort = _sorts.ContainsValue(sortOrder) ? sortOrder : _sorts[Sort.TitleAsc];
            ArtistNameSort = currentSort == _sorts[Sort.ArtistNameAsc] ? _sorts[Sort.ArtistNameDesc] : _sorts[Sort.ArtistNameAsc];
            AlbumNameSort = currentSort == _sorts[Sort.AlbumTitleAsc] ? _sorts[Sort.AlbumTitleDesc] : _sorts[Sort.AlbumTitleAsc];
            TitleSort = currentSort == _sorts[Sort.TitleAsc] ? _sorts[Sort.TitleDesc] : _sorts[Sort.TitleAsc];
            TrackNumberSort = currentSort == _sorts[Sort.TrackNumberAsc] ? _sorts[Sort.TrackNumberDesc] : _sorts[Sort.TrackNumberAsc];
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

            var result = await _dataReadService.GetTracksPage(albumId, currentFilter, sortField, sortDesc, pageIndex ?? 1, PAGE_SIZE);
            var albumNames = await _dataReadService.GetAlbumTitles();

            if (albumId.HasValue
                && !albumNames.Any(a => a.albumId == albumId.Value))
            {
                return NotFound();
            }

            var parameters = new Dictionary<string, string> {
                { ParamName.PageIndex, (pageIndex ?? 1).ToString() },
                { ParamName.SortOrder, currentSort },
                { ParamName.CurrentFilter, currentFilter },
                { ParamName.AlbumId, albumId?.ToString() },
            };

            Tracks = new PaginatedList<TrackViewDto>(result.pageItems, result.totalCount, PAGE_SIZE, pageIndex ?? 1, parameters);
            AlbumList = albumNames.Select(a => new SelectListItem(a.albumTitle, a.albumId.ToString(),
                                                                    albumId.HasValue && albumId.Value == a.albumId))
                                                                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetDetailsModal(int id)
        {
            var track = await _dataReadService.GetTrackView(id);
            if (track == null)
            {
                return NotFound();
            }

            return Partial("_DetailsModal", track);
        }

        public async Task<IActionResult> OnGetEditModal(int? id, int? albumId)
        {
            // Create item form
            if (!id.HasValue)
            {
                var createSelectOptions = await GetSelectOptions(albumId);
                return Partial("_EditModal", ((TrackEditDto)null, createSelectOptions));
            }

            // Update item form
            var track = await _dataReadService.GetTrackEdit(id.Value);
            if (track == null)
            {
                return NotFound();
            }

            var updateSelectOptions = await GetSelectOptions(track.AlbumId);
            return Partial("_EditModal", (track, updateSelectOptions));
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Item1")]TrackEditDto model)
        {
            //temp - to test handling a server-side validation error
            if (model.Title == "error")
                ModelState.AddModelError("Item1.Title", "Server-side validation error...");

            if (ModelState.IsValid)
            {
                await _dataWriteService.CreateTrack(model);

                Response.Headers.Add("HX-Trigger", "gridItemEdit");
                return new NoContentResult();
            }

            var updateSelectOptions = await GetSelectOptions(model.AlbumId);
            return Partial("_EditModal", (model, updateSelectOptions));
        }

        public async Task<IActionResult> OnPutAsync(int id, [Bind(Prefix = "Item1")]TrackEditDto model)
        {
            //temp - to test handling a server-side validation error
            if (model.Title == "error")
                ModelState.AddModelError("Item1.Title", "Server-side validation error...");

            if (ModelState.IsValid)
            {
                var result = await _dataWriteService.UpdateTrack(id, model);

                if (!result)
                    return NotFound();

                Response.Headers.Add("HX-Trigger", "gridItemEdit");
                return new NoContentResult();
            }

            var updateSelectOptions = await GetSelectOptions(model.AlbumId);
            return Partial("_EditModal", (model, updateSelectOptions));
        }

        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            var result = await _dataWriteService.DeleteTrack(id);

            if (!result)
                return NotFound();

            Response.Headers.Add("HX-Trigger", "gridItemDelete");
            return new NoContentResult();
        }

        protected async Task<List<SelectListItem>> GetSelectOptions(int? albumId = null)
        {
            var albumTitles = await _dataReadService.GetAlbumTitles();
            var albumList = albumTitles.Select(a => new SelectListItem(a.albumTitle, a.albumId.ToString(),
                                                                        albumId == a.albumId)).ToList();

            return albumList;
        }
    }
}