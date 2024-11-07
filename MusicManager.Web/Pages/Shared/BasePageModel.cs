using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MusicManager.Domain.Dtos;

namespace MusicManager.Web.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        protected PartialViewResult DetailsModal<T>(string typeDisplayName, T viewDto = null) where T : BaseViewDto
        {
            var viewData = new ViewDataDictionary(MetadataProvider, ViewData.ModelState)
            {
                Model = viewDto,
            };
            viewData["TypeDisplayName"] = typeDisplayName;

            return new PartialViewResult
            {
                ViewName = "_DetailsModalWrapper",
                ViewData = viewData
            };
        }
    }
}
