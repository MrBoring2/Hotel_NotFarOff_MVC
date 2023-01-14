using Hotel_NotFarOff.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hotel_NotFarOff.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext _db;
        public AdminController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            _logger = logger;
            _db = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoomCategoriesList()
        {
            return View();
        }
        public IActionResult RoomsList()
        {
            return View();
        }
        public IActionResult EmployeesList()
        {
            return View();
        }
        public IActionResult BookingList()
        {
            return View();
        }
    }
}
