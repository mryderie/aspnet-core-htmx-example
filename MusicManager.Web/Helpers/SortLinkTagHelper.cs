using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicManager.Web.Helpers
{
    [HtmlTargetElement("sort-link")]
    public class SortLinkTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _generator;

        [HtmlAttributeName("display-name")]
        public string DisplayName { get; set; }

        [HtmlAttributeName("route-sortOrder")]
        public string RouteSortOrder { get; set; }

        [HtmlAttributeName("page")]
        public string Page { get; set; }

        [HtmlAttributeName("route-params")]
        public IDictionary<string, string> RouteParams { get; set; }

        public SortLinkTagHelper(IHttpContextAccessor accessor, LinkGenerator generator)
        {
            _accessor = accessor;
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (string.IsNullOrWhiteSpace(DisplayName))
                throw new ArgumentException(nameof(DisplayName));

            if (string.IsNullOrWhiteSpace(RouteSortOrder))
                throw new ArgumentException(nameof(RouteSortOrder));

            if (string.IsNullOrWhiteSpace(Page))
                throw new ArgumentException(nameof(Page));

            // create a new dictionary to replace the sortOrder param
            var modifiedRouteParams = RouteParams?.Where(rp => rp.Key != ParamName.SortOrder)
                                                    .ToDictionary(rp => rp.Key, rp => rp.Value)
                                                ?? new Dictionary<string, string>();
            modifiedRouteParams[ParamName.SortOrder] = RouteSortOrder;

            var sortUrl = _generator.GetUriByPage(_accessor.HttpContext,
                                                    page: Page,
                                                    values: modifiedRouteParams);

            // determine if we're currently sorting by this column. e.g. compare "name_asc" to "name_desc"
            string sortIcon = null;
            if (RouteParams != null && RouteParams.ContainsKey(ParamName.SortOrder))
            {
                var currentSort = RouteParams[ParamName.SortOrder];
                var splitCurrentSort = currentSort.Split('_');
                var splitColumnSort = RouteSortOrder.Split('_');

                if (splitCurrentSort.Length == 2
                    && splitColumnSort.Length == 2
                    && splitCurrentSort[0] == splitColumnSort[0])
                {
                    sortIcon = splitCurrentSort[1] == "asc" ?
                                    $@"<i class=""bi-sort-up-alt"" role=""img"" aria-label=""Sort by {DisplayName}, Descending""></i>"
                                    : $@"<i class=""bi-sort-down"" role=""img"" aria-label=""Sort by {DisplayName}, Ascending""></i>";
                }
            }

            var template = $@"<a href=""{sortUrl}"">{DisplayName}{sortIcon}</a>";
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PostContent.SetHtmlContent(template);
        }
    }
}
