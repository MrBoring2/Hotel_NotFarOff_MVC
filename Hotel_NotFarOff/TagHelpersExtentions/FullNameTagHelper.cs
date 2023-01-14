using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Hotel_NotFarOff.TagHelpersExtentions
{
    public static class FullNameTagHelper
    {
        public static string GetInitials(this IHtmlHelper htmlHelper, string fullName)
        {
            var inits = Regex.Match(fullName, @"(\w+)\s+(\w+)\s+(\w+)").Groups;
            return string.Format("{0} {1}. {2}.", inits[1], inits[2].Value[0], inits[3].Value[0]);
        }
    }
}
