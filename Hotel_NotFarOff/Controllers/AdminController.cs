using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_NotFarOff.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext _db;
        public AdminController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            _logger = logger;
            _db = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoomCategoriesList()
        {
            return View();
        }
        public IActionResult RoomsList()
        {
            return View();
        }
        public IActionResult EmployeesList()
        {
            return View();
        }
        public IActionResult BookingList()
        {
            return View();
        }
        public IActionResult RoomCategoryCreate()
        {
            var viewModel = new RoomCategoryViewModel();
            List<int> subjectsIds = new List<int>();           
            viewModel.Services = _db.Services.Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()
            }).ToList();
            viewModel.ServicesIds = subjectsIds.ToArray();
            return View("RoomCategory", new TEntityPageViewModel<RoomCategoryViewModel>(Enums.PageTypeEnum.Create, viewModel));
        }
        public async Task<IActionResult> RoomCategoryEdit(int id)
        {
            var result = await GetRoomCategory(id);
            if (result != null)
            {
                var viewModel = new RoomCategoryViewModel(result);
                List<int> subjectsIds = new List<int>();
                result.Services.ToList().ForEach(p => subjectsIds.Add(p.Id));
                viewModel.Services = _db.Services.Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = p.Title,
                    Value = p.Id.ToString()
                }).ToList();
                viewModel.ServicesIds = subjectsIds.ToArray();

                return View("RoomCategory", new TEntityPageViewModel<RoomCategoryViewModel>(Enums.PageTypeEnum.Edit, viewModel));
            }
            else return RedirectToAction("RoomCategoriesList");
        }
        public async Task<IActionResult> RoomCategoryDetails(int id)
        {
            var result = await GetRoomCategory(id);
            if (result != null)
            {
                var viewModel = new RoomCategoryViewModel(result);
                List<int> subjectsIds = new List<int>();
                result.Services.ToList().ForEach(p => subjectsIds.Add(p.Id));
                viewModel.Services = _db.Services.Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = p.Title,
                    Value = p.Id.ToString()
                }).ToList();
                viewModel.ServicesIds = subjectsIds.ToArray();
                return View("RoomCategory", new TEntityPageViewModel<RoomCategoryViewModel>(Enums.PageTypeEnum.Details, viewModel));
            }
            else return RedirectToAction("RoomCategoriesList");
        }
        public IActionResult RoomCreate()
        {
            return View("Room", new TEntityPageViewModel<Room>(Enums.PageTypeEnum.Create, null));
        }
        public async Task<IActionResult> RoomEdit(int id)
        {
            var result = await GetRoom(id);
            if (result != null)
            {
                return View("Room", new TEntityPageViewModel<Room>(Enums.PageTypeEnum.Edit, result));
            }
            else return RedirectToAction("RoomsList");
        }
        public async Task<IActionResult> RoomDetails(int id)
        {
            var result = await GetRoom(id);
            if (result != null)
            {
                return View("Room", new TEntityPageViewModel<Room>(Enums.PageTypeEnum.Details, result));
            }
            else return RedirectToAction("RoomsList");
        }
        public async Task<IActionResult> BookingEdit(int id)
        {
            var result = await GetBooking(id);
            if (result != null)
            {
                return View("Booking", new TEntityPageViewModel<Booking>(Enums.PageTypeEnum.Edit, result));
            }
            else return RedirectToAction("BookingList");
        }
        public async Task<IActionResult> BookingDetails(int id)
        {
            var result = await GetBooking(id);
            if (result != null)
            {
                return View("Booking", new TEntityPageViewModel<Booking>(Enums.PageTypeEnum.Details, result));
            }
            else return RedirectToAction("BookingList");
        }
        public IActionResult EmployeeCreate()
        {
            return View("Employee", new TEntityPageViewModel<Employee>(Enums.PageTypeEnum.Create, null));
        }
        public async Task<IActionResult> EmployeeEdit(int id)
        {
            var result = await GetEmployee(id);
            if (result != null)
            {
                return View("Employee", new TEntityPageViewModel<Employee>(Enums.PageTypeEnum.Edit, result));
            }
            else return RedirectToAction("EmployeesList");
        }
        public async Task<IActionResult> EmployeeDetails(int id)
        {
            var result = await GetEmployee(id);
            if (result != null)
            {
                return View("Employee", new TEntityPageViewModel<Employee>(Enums.PageTypeEnum.Details, result));
            }
            else return RedirectToAction("EmployeesList"); ;
        }

        public async Task<RoomCategory> GetRoomCategory(int? id)
        {
            if (id == null || _db.RoomCategories == null)
            {
                return null;
            }
            var roomCategory = await _db.RoomCategories.FindAsync(id);
            await _db.Entry(roomCategory).Collection(p => p.Services).LoadAsync();
            await _db.Entry(roomCategory).Collection(p => p.RoomImages).LoadAsync();
            await _db.Entry(roomCategory).Reference(p => p.Employee).LoadAsync();
            if (roomCategory.Employee != null)
                await _db.Entry(roomCategory.Employee).Reference(p => p.Account).LoadAsync();
            //var roomCategory = await _db.RoomCategories
            //    .Include(p => p.RoomImages)
            //    .Include(p => p.Services)
            //    .Include(p => p.Employee)
            //    .ThenInclude(p => p.Account)
            //    .AsQueryable()
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (roomCategory == null)
            {
                return null;
            }

            return roomCategory;
        }
        public async Task<Room> GetRoom(int? id)
        {
            if (id == null || _db.Rooms == null)
            {
                return null;
            }

            var room = await _db.Rooms.Include(p => p.RoomCategory)
                .Include(p => p.Employee)
                .ThenInclude(p => p.Account)
                .AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return null;
            }

            return room;
        }
        public async Task<Booking> GetBooking(int? id)
        {
            if (id == null || _db.Bookings == null)
            {
                return null;
            }

            var booking = await _db.Bookings.Include(p => p.BookingStatus).Include(p => p.PaymentMethod)
                .Include(p => p.Guests)
                .Include(p => p.Room).AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return null;
            }

            return booking;
        }
        public async Task<Employee> GetEmployee(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return null;
            }

            var employee = await _db.Employees.Include(p => p.Post).Include(p => p.Account).AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return null;
            }

            return employee;
        }
    }
}
