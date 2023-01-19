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
using Microsoft.AspNetCore.Authorization;

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
            var filterRoomCategory = int.Parse(Request.Form["filterRoomCategory"].FirstOrDefault());
            var filterRoomStatus = int.Parse(Request.Form["filterRoomStatus"].FirstOrDefault());
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

            if (filterRoomCategory != 0)
            {
                data = data.Where(p => p.RoomCategoryId == filterRoomCategory);
            }

            if (filterRoomStatus != -1)
            {
                data = data.Where(p => p.IsBooked == Convert.ToBoolean(filterRoomStatus));
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                if (_db.Rooms.FirstOrDefault(p => p.RoomNumber == room.RoomNumber) != null)
                {
                    ModelState.AddModelError("RoomCategoryId", "Комната с таким номером уже существует");
                    return BadRequest("Комната с таким номером уже существует");
                }

                _db.Add(room);
                await _db.SaveChangesAsync();
                return Ok("Комната успешно создана");
            }
            var a = ModelState.Values.SelectMany(v => v.Errors);
            ModelState.AddModelError("Error", "Произошла ошибка при создании");
            return BadRequest("Произошла ошибка при создании");
        }

        // GET: RoomCategories/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Rooms == null)
            {
                return NotFound();
            }

            var room = await _db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: RoomCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var item = _db.Rooms.AsNoTracking().FirstOrDefault(p => p.RoomNumber == room.RoomNumber);
                if (item != null)
                {
                    if (item.Id != room.Id)
                    {
                        ModelState.AddModelError("RoomCategoryId", "Комната с таким номером уже существует");
                        return BadRequest("Комната с таким номером уже существует");
                    }
                }
                try
                {
                    var employeeId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(p => p.Type == "Id").Value);
                    room.EmployeeId = (await _db.Employees.FirstOrDefaultAsync(p => p.Id == employeeId)).Id;
                    room.UpdatedDate = DateTime.Now;
                    _db.Update(room);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomCategoryExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok("Комната успешно обновлена");
            }
            return BadRequest("Произошла ошибка при обновлении");
        }

        // GET: RoomCategories/Delete/5
        // POST: RoomCategories/Delete/5
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_db.Rooms == null)
            {
                return BadRequest("Entity set 'HotelNotFarOffContext.Rooms' is null.");
            }
            var room = await _db.Rooms.Include(p => p.Bookings).FirstOrDefaultAsync(p => p.Id == id);
            if (room.Bookings.Count > 0)
            {
                return BadRequest("У номера есть существующие или проведённые бронирования. Такой нмоер удалить нельзя.");
            }
            if (room != null)
            {
                _db.Rooms.Remove(room);
            }
            else
            {
                return BadRequest("Не найдена комната");
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        private bool RoomCategoryExists(int id)
        {
            return _db.Rooms.Any(e => e.Id == id);
        }


    }
}
