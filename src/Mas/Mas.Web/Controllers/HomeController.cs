using Mas.Application.UserServices;
using Mas.Application.UserServices.Dtos;
using Mas.Common;
using Mas.Core.Contants;
using Mas.Core.Enums;
using Mas.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _service;

        public HomeController(
            ILogger<HomeController> logger,
            IUserService service
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel request)
        {
             var user = await _service.Authenticate(request);
            if (user == null)
            {
                request.Message = "Sai tên đăng nhập hoặc mật khẩu.";
                return View(request);
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("Name",user.Name)
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = request.IsRemember
            });

            HttpContext.Session.SetString(ContantSessions.Name, user.Name);
            HttpContext.Session.SetString(ContantSessions.Role, ContantRole.GetRole(user.Role));

            if (string.IsNullOrEmpty(user.Image))
            {
                HttpContext.Session.SetString(ContantSessions.Avatar, ContantsFolder.Avatar);
            }
            else
            {
                HttpContext.Session.SetString(ContantSessions.Avatar, user.Image);
            }

            if(user.Role == EnumRole.Admin)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            return RedirectToAction("Sales", "Employee");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
