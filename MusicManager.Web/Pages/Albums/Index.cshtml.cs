using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Services;
using MusicManager.Web.Helpers;

namespace MusicManager.Web.Pages.Albums
{
    public class IndexModel : PageModel
    {
        private const int PAGE_SIZE = 3;
        private readonly IDataReadService _dataReadService;
        private readonly IDataWriteService _dataWriteService;

        private enum Sort
        {
            TitleAsc,
            TitleDesc,
            TrackCountAsc,
            TrackCountDesc,
            ReleaseYearAsc,
            ReleaseYearDesc,
            ArtistNameAsc,
            ArtistNameDesc,
            CreatedAsc,
            CreatedDesc,
            UpdatedAsc,
            UpdatedDesc
        }

        private Dictionary<Sort, string> _sorts = new Dictionary<Sort, string>
        {
            { Sort.TitleAsc, "title_asc" },
            { Sort.TitleDesc, "title_desc" },
            { Sort.TrackCountAsc, "trackCount_asc" },
            { Sort.TrackCountDesc, "trackCount_desc" },
            { Sort.ReleaseYearAsc, "releaseYear_asc" },
            { Sort.ReleaseYearDesc, "releaseYear_desc" },
            { Sort.ArtistNameAsc, "artistName_asc" },
            { Sort.ArtistNameDesc, "artistName_desc" },
            { Sort.CreatedAsc, "created_asc" },
            { Sort.CreatedDesc, "created_desc" },
            { Sort.UpdatedAsc, "updated_asc" },
            { Sort.UpdatedDesc, "updated_desc" },
        };

        // These Sort properties control the URL parameters for the sorting links in the table header
        public string TitleSort { get; set; }
        public string TrackCountSort { get; set; }
        public string ReleaseYearSort { get; set; }
        public string ArtistNameSort { get; set; }
        public string CreatedSort { get; set; }
        public string UpdatedSort { get; set; }

        public PaginatedList<AlbumViewDto> Albums { get; set; }
        public List<SelectListItem> ArtistList { get; set; }

        public IndexModel(IDataReadService dataReadService, IDataWriteService dataWriteService)
        {
            _dataReadService = dataReadService;
            _dataWriteService = dataWriteService;
        }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter,
                                                    string searchString, int? artistId, int? pageIndex)
        {
            // title_asc is the default sort value, so an unset or invalid parameter value will be replaced with that value
            var currentSort = _sorts.ContainsValue(sortOrder) ? sortOrder : _sorts[Sort.TitleAsc];
            TitleSort = currentSort == _sorts[Sort.TitleAsc] ? _sorts[Sort.TitleDesc] : _sorts[Sort.TitleAsc];
            TrackCountSort = currentSort == _sorts[Sort.TrackCountAsc] ? _sorts[Sort.TrackCountDesc] : _sorts[Sort.TrackCountAsc];
            ReleaseYearSort = currentSort == _sorts[Sort.ReleaseYearAsc] ? _sorts[Sort.ReleaseYearDesc] : _sorts[Sort.ReleaseYearAsc];
            ArtistNameSort = currentSort == _sorts[Sort.ArtistNameAsc] ? _sorts[Sort.ArtistNameDesc] : _sorts[Sort.ArtistNameAsc];
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

            var result = await _dataReadService.GetAlbumsPage(artistId, currentFilter, sortField, sortDesc, pageIndex ?? 1, PAGE_SIZE);
            var artistNames = await _dataReadService.GetArtistNames();

            if(artistId.HasValue
                && !artistNames.Any(a => a.artistId == artistId.Value))
            {
                return NotFound();
            }

            var parameters = new Dictionary<string, string> {
                { ParamName.PageIndex, (pageIndex ?? 1).ToString() },
                { ParamName.SortOrder, currentSort },
                { ParamName.CurrentFilter, currentFilter },
                { ParamName.ArtistId, artistId?.ToString() },
            };

            Albums = new PaginatedList<AlbumViewDto>(result.pageItems, result.totalCount, PAGE_SIZE, pageIndex ?? 1, parameters);
            ArtistList = artistNames.Select(a => new SelectListItem(a.artistName, a.artistId.ToString(),
                                                                    artistId.HasValue && artistId.Value == a.artistId))
                                                                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetDetailsModal(int id)
        {
            var album = await _dataReadService.GetAlbumView(id);
            if (album == null)
            {
                return NotFound();
            }

            return Partial("_DetailsModal", album);
        }

        public async Task<IActionResult> OnGetEditModal(int? id, int? artistId)
        {
            // Create item form
            if (!id.HasValue)
            {
                var createSelectOptions = await GetSelectOptions(artistId);
                return Partial("_EditModal", ((AlbumEditDto)null, createSelectOptions.artistList, createSelectOptions.genreList));
            }

            // Update item form
            var album = await _dataReadService.GetAlbumEdit(id.Value);
            if (album == null)
            {
                return NotFound();
            }

            var updateSelectOptions = await GetSelectOptions(album.ArtistId, album.GenreIds);
            return Partial("_EditModal", (album, updateSelectOptions.artistList, updateSelectOptions.genreList));
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Item1")]AlbumEditDto model)
        {
            //temp - to test handling a server-side validation error
            if (model.Title == "error")
                ModelState.AddModelError("Name", "Server-side validation error...");

            if (ModelState.IsValid)
            {
                await _dataWriteService.CreateAlbum(model);

                Response.Headers.Add("HX-Trigger", "gridItemEdit");
                return new NoContentResult();
            }

            return Partial("_EditModal", model);
        }

        public async Task<IActionResult> OnPutAsync(int id, [Bind(Prefix = "Item1")]AlbumEditDto model)
        {
            //temp - to test handling a server-side validation error
            if (model.Title == "error")
                ModelState.AddModelError("Name", "Server-side validation error...");

            if (ModelState.IsValid)
            {
                var result = await _dataWriteService.UpdateAlbum(id, model);

                if (!result)
                    return NotFound();

                Response.Headers.Add("HX-Trigger", "gridItemEdit");
                return new NoContentResult();
            }

            return Partial("_EditModal", model);
        }

        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            var result = await _dataWriteService.DeleteAlbum(id);

            if (!result)
                return NotFound();

            Response.Headers.Add("HX-Trigger", "gridItemDelete");
            return new NoContentResult();
        }

        protected async Task<(List<SelectListItem> artistList, List<SelectListItem> genreList)> GetSelectOptions(int? artistId = null, IList<int> genreIds = null)
        {
            var artistNames = await _dataReadService.GetArtistNames();
            var artistList = artistNames.Select(a => new SelectListItem(a.artistName, a.artistId.ToString(),
                                                                        artistId == a.artistId)).ToList();

            var genreNames = await _dataReadService.GetGenreNames();
            var genreList = genreNames.Select(a => new SelectListItem(a.genreName, a.genreId.ToString(),
                                                                        genreIds != null ? genreIds.Any(g => g == a.genreId) : false)).ToList();

            return (artistList, genreList);
        }
    }
}