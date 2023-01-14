using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Models.Entities;

namespace Hotel_NotFarOff.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HotelNotFarOffContext _db;

        public EmployeesController(HotelNotFarOffContext context)
        {
            _db = context;
        }

       
        [HttpPost]
        public async Task<IActionResult> ForAdminList()
        {
            var employees = await _db.Employees.AsQueryable().ToListAsync();
            return PartialView("_List", employees);
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
        public async Task<IActionResult> Create([Bind("Id,FullName,Passport,PhoneNumber,DateOfBirth,PostId,Gender")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Add(employee);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_db.Posts, "Id", "Title", employee.PostId);
            return View(employee);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Passport,PhoneNumber,DateOfBirth,PostId,Gender")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_db.Posts, "Id", "Title", employee.PostId);
            return View(employee);
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Employees == null)
            {
                return Problem("Entity set 'HotelNotFarOffContext.Employees'  is null.");
            }
            var employee = await _db.Employees.FindAsync(id);
            if (employee != null)
            {
                _db.Employees.Remove(employee);
            }
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _db.Employees.Any(e => e.Id == id);
        }
    }
}
