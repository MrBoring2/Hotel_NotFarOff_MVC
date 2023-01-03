using Microsoft.AspNetCore.Mvc;

namespace Hotel_NotFarOff.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
