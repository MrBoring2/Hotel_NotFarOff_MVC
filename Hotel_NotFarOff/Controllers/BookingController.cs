using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_NotFarOff.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext _db;
        public BookingController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            _logger = logger;
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoomList(int guestCount)
        {
            List<RoomCategory> rooms;
            if (guestCount > 0)
            {
                rooms = await _db.RoomCategories.Where(p => p.Rooms.Any(p => p.IsBooked == false)).Where(p => p.NumbeOfSeats >= guestCount).AsQueryable().ToListAsync();
            }
            else
            {
                rooms = await _db.RoomCategories.Where(p => p.Rooms.Any(p => p.IsBooked == false)).AsQueryable().ToListAsync();
            }

            return PartialView("_RoomList", rooms.Select(p => new RoomInListViewModel(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage)));
        }

        [HttpPost]
        public async Task<IActionResult> Confirm([FromBody] JObject checkIn)
        {
            var bookingData = JsonConvert.DeserializeObject<BookingData>(checkIn.ToString());
            bookingData.CheckIn = bookingData.CheckIn.ToLocalTime();
            bookingData.CheckOut = bookingData.CheckOut.ToLocalTime();
            var roomCategory = await _db.RoomCategories.FirstOrDefaultAsync(p => p.Id == bookingData.RoomCategoryId);
            return PartialView("_Confirm", new BookingViewModel(bookingData, roomCategory));

        }
    }
}
