using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel_NotFarOff.TagHelpersExtentions
{
    public static class RoomStatusTagHelper
    {
        public static string IsBooked(this IHtmlHelper htmlHelper, bool isBooked)
        {

            return isBooked ? "Забронирован" : "Свободен";
        }
    }
}
