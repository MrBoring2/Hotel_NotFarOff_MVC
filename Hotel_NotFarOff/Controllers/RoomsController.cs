using Hotel_NotFarOff.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hotel_NotFarOff.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext db;
        public RoomsController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            db = context;
        }
        public async Task<IActionResult> RoomsList()
        {
            return View(await db.RoomCategories.ToListAsync());
        }
    }
}
