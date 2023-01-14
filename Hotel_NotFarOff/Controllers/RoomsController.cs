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


        [HttpPost]
        public async Task<IActionResult> ForAdminList()
        {
            var rooms = await _db.Rooms.AsQueryable().ToListAsync();
            return PartialView("_List", rooms);
        }
        [HttpPost]
        public async Task<ActionResult<ICollection<Room>>> GetList()
        {
            var rooms = await _db.Rooms.AsQueryable().ToListAsync();
            return rooms;
        }

        // GET: RoomCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Rooms == null)
            {
                return NotFound();
            }

            var room = await _db.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
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

        private bool RoomCategoryExists(int id)
        {
            return _db.RoomCategories.Any(e => e.Id == id);
        }


    }
}
