using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Enums;
using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_NotFarOff.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext _db;


        public BookingController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            _logger = logger;
            _db = context;
        }
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync(BookingData bookingData)
        {
            if (ModelState.IsValid)
            {
                return View(new GuestBookingViewModel(bookingData, await _db.PaymentMethods.ToListAsync()));

            }
            else
            {
                return View("../Home/Index");
                //if (bookingData.CheckOut <= bookingData.CheckIn)
                //{
                //    ModelState.AddModelError("Error", "Дата выезда должна быть больше даты заезда.");
                //    return View("../Home/Index");
                //}
            }
        }
        [HttpPost]
        public async Task<IActionResult> ForAdminList()
        {
            var bookings = await _db.Bookings.AsQueryable().ToListAsync();
            return PartialView("_List", bookings);
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
            var filterBookingStatus = int.Parse(Request.Form["filterBookingStatus"].FirstOrDefault());
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var data = _db.Bookings.Include(p => p.Room).Include(p => p.BookingStatus).AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.TenantFio.ToLower().Contains(searchValue.ToLower()) || x.Email.ToLower().ToLower().Contains(searchValue.ToLower())
                                             || x.PhoneNumber.ToLower().Contains(searchValue.ToLower()) || x.Id.ToString().ToLower().Contains(searchValue.ToLower()));
            }

            if (filterBookingStatus != 0)
            {
                data = data.Where(p => p.BookingStatusId == filterBookingStatus);
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
        public JsonResult GetStatusesTitlesList()
        {
            return Json(_db.BookingStatuses.AsQueryable().Select(p => new { id = p.Id, title = p.Title }));
        }


        [AllowAnonymous]
        public IActionResult Create() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm([FromBody] JObject checkIn)
        {
            var bookingData = JsonConvert.DeserializeObject<BookingData>(checkIn.ToString());
            bookingData.CheckIn = bookingData.CheckIn.ToLocalTime();
            bookingData.CheckOut = bookingData.CheckOut.ToLocalTime();
            var roomCategory = await _db.RoomCategories.FirstOrDefaultAsync(p => p.Id == bookingData.RoomCategoryId);
            return PartialView("_Confirm", new GuestBookingViewModel(bookingData, roomCategory, await _db.PaymentMethods.ToListAsync()));

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(GuestBookingViewModel bookingViewModel)
        {
            if (ModelState.IsValid)
            {
                bookingViewModel.BookingData.CheckIn.AddHours(14);
                bookingViewModel.BookingData.CheckOut.AddHours(12);

                var booking = new Booking();
                booking.BookingStatusId = (int)BookingStatuses.InProcess;
                booking.PhoneNumber = bookingViewModel.TenantPhone;
                booking.CheckIn = bookingViewModel.BookingData.CheckIn;
                booking.CheckOut = bookingViewModel.BookingData.CheckOut;
                booking.TenantFio = bookingViewModel.TenantFullName;
                booking.Email = bookingViewModel.TenantEmail;
                booking.PaymentMethodId = bookingViewModel.SelectedPaymentMethodId;
                booking.Guests = new List<Guest>(bookingViewModel.Guests.Select(p => new Guest { FullName = p.FullName }));
                booking.CreatedDate = DateTime.Now;
                var room = (await _db.Rooms.FirstOrDefaultAsync(p => p.RoomCategoryId == bookingViewModel.BookingData.RoomCategoryId && p.IsBooked == false));
                if (room != default && room != null)
                {
                    booking.RoomId = room.Id;
                    room.IsBooked = true;
                    _db.Entry(room).State = EntityState.Modified;

                    _db.Bookings.Add(booking);
                    await _db.SaveChangesAsync();
                    return Ok("Бронирование успешно сформировано. С вами скоро свяжутся. Вы можете связаться с нами по нашим контактам.");
                }
                else
                {
                    return BadRequest("Номера этой категории закончились.");
                }
            }

            return BadRequest("Ввведены неккоректные данные.");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var bookingDb = await _db.Bookings.FirstOrDefaultAsync(p => p.Id == id);
                    bookingDb.BookingStatusId = booking.BookingStatusId;
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok("Бронирование успешно изменёнено");
            }
            return BadRequest("Не все данные введены корректно");
        }
        private bool BookingExists(int id)
        {
            return _db.Bookings.Any(e => e.Id == id);
        }
    }
}
