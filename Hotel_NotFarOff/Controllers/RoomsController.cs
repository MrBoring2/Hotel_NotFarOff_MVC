using Hotel_NotFarOff.Compressions;
using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_NotFarOff.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext _db;
        public RoomsController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            _logger = logger;
            _db = context;
        }
        public async Task<IActionResult> List()
        {
            return View(await _db.RoomCategories.Select(p => new RoomInListViewModel(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, p.Rooms.Count(p => p.IsBooked == false))).AsNoTracking().AsQueryable().ToListAsync());
        }
        public async Task<IActionResult> Details(int roomCategoryId)
        {
            var room = await _db.RoomCategories.FirstOrDefaultAsync(p => p.Id == roomCategoryId);
            return View(new RoomCategoryViewModel(room));
        }

        public FileResult GetFileFromBytes(int id)
        {
            return File(_db.RoomCategories.FirstOrDefault(p => p.Id == id).MainImage, "image/png");
        }
        public FileResult GetRoomImage(int id, int imageNumber)
        {
            return File(_db.RoomImages.Where(p => p.RoomCategoryId == id).Skip(imageNumber - 1).FirstOrDefault().Image, "image/png");
        }

    }
}
