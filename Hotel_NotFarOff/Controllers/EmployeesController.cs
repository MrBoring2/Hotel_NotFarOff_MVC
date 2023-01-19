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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Hotel_NotFarOff.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly HotelNotFarOffContext _db;

        public EmployeesController(HotelNotFarOffContext context)
        {
            _db = context;
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
            var filterEmployeePost = int.Parse(Request.Form["filterEmployeePost"].FirstOrDefault());
            var filterEmployeeGender = int.Parse(Request.Form["filterEmployeeGender"].FirstOrDefault());
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var data = _db.Employees.Include(p => p.Post).Include(p => p.Account).AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.FullName.ToLower().Contains(searchValue.ToLower()) || x.Passport.ToLower().Contains(searchValue.ToLower())
                                              || x.PhoneNumber.ToLower().Contains(searchValue.ToLower()) || (x.Account != null && x.Account.Login.ToLower().Contains(searchValue.ToLower())));
            }

            if (filterEmployeePost != 0)
            {
                data = data.Where(p => p.PostId == filterEmployeePost);
            }

            if (filterEmployeeGender != 0)
            {
                if (filterEmployeeGender == 1)
                {
                    data = data.Where(p => p.Gender == "мужской");
                }
                else if (filterEmployeeGender == 2)
                {
                    data = data.Where(p => p.Gender == "женский");
                }
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
        public JsonResult GetPostsTitlesList()
        {
            return Json(_db.Posts.AsQueryable().Select(p => new { id = p.Id, title = p.Title }));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees
                .Include(e => e.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_db.Posts, "Id", "Title");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (_db.Employees.FirstOrDefault(p => p.Account.Login == employee.Account.Login) != null)
                {
                    return BadRequest("Пользователь с таким логином уже существует");
                }
                _db.Add(employee);
                await _db.SaveChangesAsync();
                return Ok("Сотрудник успешно создан");
            }
            return BadRequest("Не все данные введены корректно");
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_db.Posts, "Id", "Title", employee.PostId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employeeId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(p => p.Type == "Id").Value);
                    if (id == employeeId)
                    {
                        return BadRequest("Нельзя редактировать пользователя, который уже авторизовался");
                    }


                    var employeeDb = _db.Employees.Include(p => p.Account).AsNoTracking().FirstOrDefault(p => p.Account.Login == employee.Account.Login);
                    if (employeeDb != null && employeeDb.Id != employee.Id)
                    {
                        return BadRequest("Пользователь с таким логином уже существует");
                    }


                    employee.UpdatedDate = DateTime.Now;
                    _db.Update(employee);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok("Сотрудник успешно изменён");
            }
            return BadRequest("Не все данные введены корректно");
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees
                .Include(e => e.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_db.Employees == null)
            {
                return Problem("Entity set 'HotelNotFarOffContext.Employees'  is null.");
            }
            var employee = await _db.Employees.FindAsync(id);
            if (employee != null)
            {
                var employeeId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(p => p.Type == "Id").Value);
                if (id == employeeId)
                {
                    return BadRequest("Нельзя удалять пользователя, который уже авторизовался");
                }
                _db.Employees.Remove(employee);
            }

            await _db.SaveChangesAsync();
            return Ok("Сотрудник успешно удалён");
        }

        private bool EmployeeExists(int id)
        {
            return _db.Employees.Any(e => e.Id == id);
        }
    }
}
