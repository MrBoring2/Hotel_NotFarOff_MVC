using Hotel_NotFarOff.Contexts;
using Hotel_NotFarOff.Enums;
using Hotel_NotFarOff.Models;
using Hotel_NotFarOff.Models.Entities;
using Hotel_NotFarOff.Services;
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
using System.Linq;
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
        /// <summary>
        /// Метод контроллера авторизации
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// Метод контроллера авторизации
        /// </summary>
        /// <param name="vm">ViewModel передаваемая с клиента</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var employee = await _db.Employees.Include(p => p.Account).Include(p => p.Post).AsQueryable().FirstOrDefaultAsync(p => p.Account.Login.Equals(vm.Login) && p.Account.Password.Equals(vm.Password));

                //TODO: проверка пароля, загрузка пользователя из БД, и т.д. и т.п.

                if (employee != null)
                {
                    if (employee.PostId != (int)Posts.Manager && employee.PostId != (int)Posts.Admin)
                    {
                        ModelState.AddModelError("", "У вас нет прав авторизоваться здесь.");
                        return View(vm);
                    }

                    var user = new User { Id = employee.Id.ToString(), Login = employee.Account.Login, PostId = employee.PostId };
                    if (!ConnectingService.IsUserAuth(user.Id))
                    {
                        ConnectingService.AddUser(user);
                        await Authenticate(employee);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким логином уже авторизирован");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверные логин и(или) пароль");
                }
            }
            return View(vm);
        }
        /// <summary>
        /// Метод аутентификации
        /// </summary>
        /// <param name="employee">Сотрудник, который авторизуется</param>
        /// <returns></returns>
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
                    principal);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //        principal,
            //        new AuthenticationProperties
            //        {
            //            //ExpiresUtc = DateTime.UtcNow.AddMinutes(120)
            //        });

            _logger.LogInformation(4, "Пользователь авторизовался.");
        }
        /// <summary>
        /// Метод выхода
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            var userId = HttpContext.User.Claims.First(p => p.Type == "Id").Value;
            if (ConnectingService.RemoveUser(userId))
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}
