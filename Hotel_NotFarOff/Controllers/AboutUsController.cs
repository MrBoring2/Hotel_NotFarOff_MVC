using Microsoft.AspNetCore.Mvc;

namespace Hotel_NotFarOff.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
