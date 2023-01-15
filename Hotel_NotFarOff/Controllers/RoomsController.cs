using Hotel_NotFarOff.Compressions;
using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        public JsonResult GetList()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            var draw = Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var data = _db.Rooms.Include(p => p.RoomCategory).AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.RoomNumber.ToString().ToLower().Contains(searchValue.ToLower()) || x.RoomCategory.Title.ToLower().Contains(searchValue.ToLower()));
            }
            // get total count of records after search
            filterRecord = data.Count();
            //sort data
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                data = data.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            //pagination
            var empList = data.Skip(skip).Take(pageSize);
            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = empList.ToList()
            };

            return Json(returnObj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore! });
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
