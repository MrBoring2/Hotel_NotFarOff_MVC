using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Hotel_NotFarOff.Models;

namespace Hotel_NotFarOff.Controllers
{
    [Authorize]
    public class RoomCategoriesController : Controller
    {
        private readonly HotelNotFarOffContext _db;

        public RoomCategoriesController(HotelNotFarOffContext context)
        {
            _db = context;
        }

        // GET: RoomCategories
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.RoomCategories.ToListAsync());
        //}
        [HttpGet]
        [AllowAnonymous]
        public IActionResult List()
        {
            return View(_db.RoomCategories.AsQueryable().Include(p => p.Rooms).Select(p => new RoomInList(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, p.Rooms.AsEnumerable().Count(p => p.IsBooked == false))));
        }

        [HttpPost]
        public IActionResult ForAdminList()
        {
            var roomCategories = _db.RoomCategories.AsQueryable().Select(p =>
                    new RoomInList(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, 0));
            return PartialView("_ForAdminList", roomCategories);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult List(BookingViewModel res)
        {
            IQueryable<RoomCategory> rooms;
            int roomCategoryId = res.BookingData.RoomCategoryId;
            int guestCount = res.BookingData.AdultCount + res.BookingData.ChildCount;
            if (roomCategoryId == 0)
            {
                if (guestCount > 0)
                {
                    rooms = _db.RoomCategories.AsQueryable().Include(p => p.Rooms).Where(p => p.Rooms.Any(p => p.IsBooked == false)).Where(p => p.NumbeOfSeats >= guestCount);
                }
                else
                {
                    rooms = _db.RoomCategories.AsQueryable().Include(p => p.Rooms).Where(p => p.Rooms.Any(p => p.IsBooked == false));
                }
            }
            else
            {
                rooms = _db.RoomCategories.AsQueryable().Include(p => p.Rooms).Where(p => p.Id == roomCategoryId).Where(p => p.Rooms.Any(p => p.IsBooked == false)).Where(p => p.NumbeOfSeats >= guestCount);
            }
            if (rooms.Count() > 0)
            {
                var roomViewModel = rooms.Select(p => new RoomInList(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, p.Rooms.Count(p => p.IsBooked == false)));
                return PartialView("_RoomList", roomViewModel);
            }
            else
            {
                return Content("<h2 style='text - align: center'>Номера с такими параметрами поиска не найдены или все комнаты данной категории заняты.</h2>");
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.RoomCategories == null)
            {
                return NotFound();
            }

            var room = await _db.RoomCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(new RoomCategoryViewModel(room));
        }

        // GET: RoomCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PricePerDay,RoomCount,NumbeOfSeats,RoomSize,ShortDescription,Description,Services,MainImage")] RoomCategory roomCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Add(roomCategory);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomCategory);
        }

        // GET: RoomCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.RoomCategories == null)
            {
                return NotFound();
            }

            var roomCategory = await _db.RoomCategories.FindAsync(id);
            if (roomCategory == null)
            {
                return NotFound();
            }
            return View(roomCategory);
        }

        // POST: RoomCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PricePerDay,RoomCount,NumbeOfSeats,RoomSize,ShortDescription,Description,Services,MainImage")] RoomCategory roomCategory)
        {
            if (id != roomCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(roomCategory);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomCategoryExists(roomCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roomCategory);
        }

        // GET: RoomCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.RoomCategories == null)
            {
                return NotFound();
            }

            var roomCategory = await _db.RoomCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(roomCategory);
        }

        // POST: RoomCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.RoomCategories == null)
            {
                return Problem("Entity set 'HotelNotFarOffContext.RoomCategories'  is null.");
            }
            var roomCategory = await _db.RoomCategories.FindAsync(id);
            if (roomCategory != null)
            {
                _db.RoomCategories.Remove(roomCategory);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public FileResult GetFileFromBytes(int id)
        {
            return File(_db.RoomCategories.FirstOrDefault(p => p.Id == id).MainImage, "image/png");
        }
        [AllowAnonymous]
        public FileResult GetRoomImage(int id, int imageNumber)
        {
            return File(_db.RoomImages.Where(p => p.RoomCategoryId == id).Skip(imageNumber - 1).FirstOrDefault().Image, "image/png");
        }
        private bool RoomCategoryExists(int id)
        {
            return _db.RoomCategories.Any(e => e.Id == id);
        }


    }
}
