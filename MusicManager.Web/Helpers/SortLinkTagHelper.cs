using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace MusicManager.Web.Helpers
{
    [HtmlTargetElement("sort-link")]
    public class SortLinkTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _generator;

        public SortLinkTagHelper(IHttpContextAccessor accessor, LinkGenerator generator)
        {
            _accessor = accessor;
            _generator = generator;
        }
        /*
        <sort-header display-name="Model.Artists[0].Name"
                    current-sort-param="@Model.CurrentSort"
                    page="./Index"        
                    route-sortOrder="@Model.NameSort"
                    route-currentFilter="@Model.Artists.PagingData.CurrentFilter" />




        <a asp-page="./Index" asp-route-sortOrder="@Model.NameSort" asp-route-currentFilter="@Model.Artists.PagingData.CurrentFilter">
                    @Html.DisplayNameFor(model => model.Artists[0].Name)

                    @if (Model.NameSort.EndsWith("_asc"))
                    {
                        <i class="bi-sort-down" role="img" aria-label="Sort Ascending"></i>
                    }
                    else
                    {
                        <i class="bi-sort-up-alt" role="img" aria-label="Sort Descending"></i>
                    }

                </a>

        */
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // extract the mandatory attribute values
            if (!output.Attributes.TryGetAttribute("display-name", out var displayNameAttr))
                throw new ApplicationException(@"sort-link tag helper missing ""display-name"" attribute");

            var displayName = displayNameAttr.Value as HtmlString;

            if (!output.Attributes.TryGetAttribute("page", out var pageAttr))
                throw new ApplicationException(@"sort-link tag helper missing ""page"" attribute");

            var page = pageAttr.Value as HtmlString;

            if (!output.Attributes.TryGetAttribute("current-sort", out var currentSortAttr))
                throw new ApplicationException(@"sort-link tag helper missing ""current-sort"" attribute");

            var currentSort = currentSortAttr.Value as HtmlString;
            
            if (!output.Attributes.TryGetAttribute("route-sortOrder", out var routeSortOrderAttr))
                throw new ApplicationException(@"sort-link tag helper missing ""route-sortOrder"" attribute");

            var routeSortOrder = routeSortOrderAttr.Value as HtmlString;

            // optional attribute - may not be set
            output.Attributes.TryGetAttribute("route-currentFilter", out var routeCurrentFilterAttr);


            var sortUrl = _generator.GetUriByPage(_accessor.HttpContext,
                                                    page: page.Value,
                                                    handler: null,
                                                    values: new
                                                    {
                                                        sortOrder = routeSortOrder.Value,
                                                        currentFilter = routeCurrentFilterAttr?.Value
                                                    });

            // determine if we're currently sorting by this column. e.g. compare "name_asc" to "name_desc"
            var splitCurrentSort = currentSort.Value.Split('_');
            if(splitCurrentSort.Length != 2)
            {
                throw new ApplicationException($@"sort-link tag helper invalid ""current-sort"" value: ""{currentSort.Value}""");
            }
            var splitColumnSort = routeSortOrder.Value.Split('_');
            if (splitColumnSort.Length != 2)
            {
                throw new ApplicationException($@"sort-link tag helper invalid ""route-sortOrder"" value: ""{routeSortOrder.Value}""");
            }

            string sortIcon = null;
            if (splitCurrentSort[0] == splitColumnSort[0])
            {
                sortIcon = splitCurrentSort[1] == "asc" ?
                                $@"<i class=""bi-sort-up-alt"" role=""img"" aria-label=""Sort by {displayName.Value}, Descending""></i>"
                                : $@"<i class=""bi-sort-down"" role=""img"" aria-label=""Sort by {displayName.Value}, Ascending""></i>";
            }

            var template = $@"<a href=""{sortUrl}"">{displayName.Value}{sortIcon}</a>";
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PostContent.SetHtmlContent(template);
        }
    }
}
