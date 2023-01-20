using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Hotel_NotFarOff.Models;
using Newtonsoft.Json;
using System.IO;

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
            return View(_db.RoomCategories.Include(p => p.Rooms).AsQueryable()
                .Select(p => new RoomCategoryInList(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, p.Rooms
                .AsQueryable().Count(p => p.IsBooked == false))));
        }

        [HttpPost]
        public JsonResult ForAdminList()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            var draw = Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var data = _db.RoomCategories.AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.Title.ToLower().Contains(searchValue.ToLower()) || x.RoomSize.ToString().ToLower().Contains(searchValue.ToLower())
                                                 || x.PricePerDay.ToString().ToLower().Contains(searchValue.ToLower()) || x.Id.ToString().ToLower().Contains(searchValue.ToLower()));
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
        [HttpGet]
        public JsonResult GetTitlesList()
        {
            return Json(_db.RoomCategories.AsQueryable().Select(p => new { id = p.Id, title = p.Title }));
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult List(GuestBookingViewModel res)
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
                var roomViewModel = rooms.Select(p => new RoomCategoryInList(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, p.Rooms.AsQueryable().Count(p => p.IsBooked == false)));
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

            var roomCategory = await _db.RoomCategories.Include(p => p.Services).AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(new RoomCategoryInListViewModel(roomCategory));
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
        public async Task<IActionResult> Create(RoomCategoryViewModel roomCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var roomCategory = new RoomCategory();
                List<Service> services = new List<Service>();
                roomCategory.NumbeOfSeats = roomCategoryViewModel.NumbeOfSeats;
                roomCategory.PricePerDay = (decimal)roomCategoryViewModel.PricePerDay;
                roomCategory.Description = roomCategoryViewModel.Description;
                roomCategory.ShortDescription = roomCategoryViewModel.ShortDescription;
                roomCategory.Title = roomCategoryViewModel.Title;
                roomCategory.RoomSize = roomCategoryViewModel.RoomSize;
                roomCategory.RoomCount = roomCategoryViewModel.RoomCount;
                if (roomCategoryViewModel.ServicesIds != null && roomCategoryViewModel.ServicesIds.Length > 0)
                {
                    foreach (var serviceid in roomCategoryViewModel.ServicesIds)
                    {
                        services.Add(_db.Services.Find(serviceid));
                    }
                    roomCategory.Services = services;
                }
                else
                {
                    return BadRequest("Сервисы не выбраны");
                }

                if (roomCategoryViewModel.MainImageFile != null)
                {
                    using (var binaryReader = new BinaryReader(roomCategoryViewModel.MainImageFile.OpenReadStream()))
                    {
                        roomCategory.MainImage = binaryReader.ReadBytes((int)roomCategoryViewModel.MainImageFile.Length);
                    }
                }
                else
                {
                    return BadRequest("Не выбрано главное изображение");
                }

                if (roomCategoryViewModel.RoomImage1 != null)
                {
                    using (var binaryReader = new BinaryReader(roomCategoryViewModel.RoomImage1.OpenReadStream()))
                    {
                        roomCategory.RoomImages.Add(new RoomImage { Image = binaryReader.ReadBytes((int)roomCategoryViewModel.RoomImage1.Length) });
                    }
                }
                else
                {
                    return BadRequest("Не выбраны все 3 дополнительных изображения");
                }
                if (roomCategoryViewModel.RoomImage2 != null)
                {
                    using (var binaryReader = new BinaryReader(roomCategoryViewModel.RoomImage2.OpenReadStream()))
                    {
                        roomCategory.RoomImages.Add(new RoomImage { Image = binaryReader.ReadBytes((int)roomCategoryViewModel.RoomImage2.Length) });
                    }
                }
                else
                {
                    return BadRequest("Не выбраны все 3 дополнительных изображения");
                }
                if (roomCategoryViewModel.RoomImage3 != null)
                {
                    using (var binaryReader = new BinaryReader(roomCategoryViewModel.RoomImage3.OpenReadStream()))
                    {
                        roomCategory.RoomImages.Add(new RoomImage { Image = binaryReader.ReadBytes((int)roomCategoryViewModel.RoomImage3.Length) });
                    }
                }
                else
                {
                    return BadRequest("Не выбраны все 3 дополнительных изображения");
                }

                _db.Add(roomCategory);
                await _db.SaveChangesAsync();
                return Ok("Категория номера успешно создана");
            }
            return Ok("Не все данные введены корректно");
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
        public async Task<IActionResult> Edit(int id, RoomCategoryViewModel roomCategoryViewModel)
        {
            if (id != roomCategoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var roomCategory = await _db.RoomCategories.FindAsync(id);
                    List<Service> services = new List<Service>();
                    roomCategory.NumbeOfSeats = roomCategoryViewModel.NumbeOfSeats;
                    roomCategory.Description = roomCategoryViewModel.Description;
                    roomCategory.ShortDescription = roomCategoryViewModel.ShortDescription;
                    roomCategory.Title = roomCategoryViewModel.Title;
                    roomCategory.RoomSize = roomCategoryViewModel.RoomSize;
                    roomCategory.RoomCount = roomCategoryViewModel.RoomCount;
                    roomCategory.UpdatedDate = DateTime.Now;
                    var employeeId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(p => p.Type == "Id").Value);
                    roomCategory.EmployeeId = (await _db.Employees.FirstOrDefaultAsync(p => p.Id == employeeId)).Id;

                    if (roomCategoryViewModel.ServicesIds.Length > 0)
                    {
                        await _db.Entry(roomCategory).Collection(p => p.Services).LoadAsync();
                        //roomCategory.Services.Clear();
                        //services = new List<Service>();

                        foreach (var serviceid in roomCategoryViewModel.ServicesIds)
                        {
                            if (!roomCategory.Services.Any(p => p.Id == serviceid))
                            {
                                roomCategory.Services.Add(_db.Services.Find(serviceid));
                            }
                        }
                        foreach (var serviceid in roomCategory.Services)
                        {
                            if (!roomCategoryViewModel.ServicesIds.Any(p => p == serviceid.Id))
                            {
                                roomCategory.Services.Remove(serviceid);
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("Сервисы не выбраны");
                    }


                    if (roomCategoryViewModel.MainImageFile != null)
                    {
                        using (var binaryReader = new BinaryReader(roomCategoryViewModel.MainImageFile.OpenReadStream()))
                        {
                            roomCategory.MainImage = binaryReader.ReadBytes((int)roomCategoryViewModel.MainImageFile.Length);
                        }
                    }
                    if (roomCategoryViewModel.RoomImage1 != null || roomCategoryViewModel.RoomImage2 != null || roomCategoryViewModel.RoomImage2 != null)
                    {
                        await _db.Entry(roomCategory).Reference(p => p.RoomImages).LoadAsync();
                    }


                    if (roomCategoryViewModel.RoomImage1 != null)
                    {

                        using (var binaryReader = new BinaryReader(roomCategoryViewModel.RoomImage1.OpenReadStream()))
                        {
                            roomCategory.RoomImages[0].Image = binaryReader.ReadBytes((int)roomCategoryViewModel.RoomImage1.Length);
                        }
                    }

                    if (roomCategoryViewModel.RoomImage2 != null)
                    {
                        using (var binaryReader = new BinaryReader(roomCategoryViewModel.RoomImage2.OpenReadStream()))
                        {
                            roomCategory.RoomImages[1].Image = binaryReader.ReadBytes((int)roomCategoryViewModel.RoomImage2.Length);
                        }
                    }

                    if (roomCategoryViewModel.RoomImage3 != null)
                    {
                        using (var binaryReader = new BinaryReader(roomCategoryViewModel.RoomImage3.OpenReadStream()))
                        {
                            roomCategory.RoomImages[2].Image = binaryReader.ReadBytes((int)roomCategoryViewModel.RoomImage3.Length);
                        }
                    }

                    _db.Entry(roomCategory).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomCategoryExists(roomCategoryViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok("Категория номера успешно изменена");
            }
            var a = ModelState.Values.SelectMany(v => v.Errors);

            return BadRequest("Не все данные введены корректно");
        }

        // GET: RoomCategories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _db.RoomCategories == null)
        //    {
        //        return NotFound();
        //    }

        //    var roomCategory = await _db.RoomCategories
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (roomCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(roomCategory);
        //}

        // POST: RoomCategories/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_db.RoomCategories == null)
            {
                return Problem("Entity set 'HotelNotFarOffContext.RoomCategories'  is null.");
            }
            var roomCategory = await _db.RoomCategories.Include(p => p.Rooms).FirstOrDefaultAsync(p => p.Id == id);
            if (roomCategory != null)
            {
                if (roomCategory.Rooms.Count() > 0)
                {
                    return BadRequest("Категория номера содержит существующие номера. Такую категорию удалить нельзя.");
                }
                _db.RoomCategories.Remove(roomCategory);
            }

            await _db.SaveChangesAsync();
            return Ok("Категория номера успешно удалена");
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
