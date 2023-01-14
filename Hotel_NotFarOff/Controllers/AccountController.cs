using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Enums;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotel_NotFarOff.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelNotFarOffContext _db;
        public AccountController(ILogger<HomeController> logger, HotelNotFarOffContext context)
        {
            _logger = logger;
            _db = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var employee = await _db.Employees.FirstOrDefaultAsync(p => p.Account.Login.Equals(vm.Login) && p.Account.Password.Equals(vm.Password));

                //TODO: проверка пароля, загрузка пользователя из БД, и т.д. и т.п.

                if (employee != null)
                {
                    if (employee.PostId != (int)Posts.Manager && employee.PostId != (int)Posts.Admin)
                    {
                        ModelState.AddModelError("", "У вас нет прав авторизоваться здесь.");
                        return View(vm);
                    }
                    await Authenticate(employee);

                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError("", "Неверные логин и(или) пароль");
            }
            return View(vm);
        }
        private async Task Authenticate(Employee employee)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.Account.Login),
                new Claim("FullName", employee.FullName),
                new Claim("PostId", employee.PostId.ToString()),
                new Claim("Id", employee.Id.ToString())
            };
            // создаем объект ClaimsIdentity

            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var principal = new ClaimsPrincipal(identity);

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    });

            _logger.LogInformation(4, "Пользователь авторизовался.");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}
