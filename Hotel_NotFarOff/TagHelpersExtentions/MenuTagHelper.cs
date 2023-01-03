using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Hotel_NotFarOff.TagHelpersExtentions
{
    public static class MenuTagHelper 
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController && (action == routeAction || routeAction == "Details"));

            return returnActive ? "active" : "";
        }
    }
}
