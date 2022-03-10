using Mas.Application.ProductServices;
using Mas.Core;
using Mas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IProductService _service;

        public EmployeeController(IProductService service)
        {
            _service = service;
        }
        public IActionResult Sales()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProduct(string barcode)
        {
            var prod = await _service.GetProductAsync(barcode);

            return Json(prod);
        }
    }
}
