using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Enums;
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
using System.Net.Http;
using System.Text;
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

        public async Task<IActionResult> IndexAsync(BookingData bookingData)
        {
            if (ModelState.IsValid)
            {
                return View(new BookingViewModel(bookingData, await _db.PaymentMethods.ToListAsync()));

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
        public async Task<IActionResult> RoomList(BookingViewModel res)
        {
            var a = HttpContext;
            List<RoomCategory> rooms;
            int roomCategoryId = res.BookingData.RoomCategoryId;
            int guestCount = res.BookingData.AdultCount + res.BookingData.ChildCount;
            if (roomCategoryId == 0)
            {
                if (guestCount > 0)
                {
                    rooms = await _db.RoomCategories.Where(p => p.Rooms.Any(p => p.IsBooked == false)).Where(p => p.NumbeOfSeats >= guestCount).AsQueryable().ToListAsync();
                }
                else
                {
                    rooms = await _db.RoomCategories.Where(p => p.Rooms.Any(p => p.IsBooked == false)).AsQueryable().ToListAsync();
                }
            }
            else
            {
                rooms = await _db.RoomCategories.Where(p => p.Id == roomCategoryId).Where(p => p.Rooms.Any(p => p.IsBooked == false)).Where(p => p.NumbeOfSeats >= guestCount).AsQueryable().ToListAsync();
            }

            if (rooms.Count > 0)
            {
                var roomViewModel = rooms.Select(p => new RoomInListViewModel(p.Id, p.Title, p.PricePerDay, p.RoomCount, p.NumbeOfSeats, p.RoomSize, p.ShortDescription, p.MainImage, p.Rooms.Count(p => p.IsBooked == false))).ToList();
                return PartialView("_RoomList", roomViewModel);
            }
            else
            {
                return Content("<h2 style='text - align: center'>Номера с такими параметрами поиска не найдены или все комнаты данной категории заняты.</h2>");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Confirm([FromBody] JObject checkIn)
        {
            var bookingData = JsonConvert.DeserializeObject<BookingData>(checkIn.ToString());
            bookingData.CheckIn = bookingData.CheckIn.ToLocalTime();
            bookingData.CheckOut = bookingData.CheckOut.ToLocalTime();
            var roomCategory = await _db.RoomCategories.FirstOrDefaultAsync(p => p.Id == bookingData.RoomCategoryId);
            return PartialView("_Confirm", new BookingViewModel(bookingData, roomCategory, await _db.PaymentMethods.ToListAsync()));

        }

        public IActionResult Create() => View();


        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel bookingViewModel)
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
    }
}
