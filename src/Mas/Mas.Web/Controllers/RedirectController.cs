using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult UnAuthorize()
        {
            return View();
        }
    }
}
