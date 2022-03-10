using Mas.Application.CategoryServices;
using Mas.Application.ProductServices;
using Mas.Application.ProductServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly ICategoryService _catSerivce;
        private readonly IProductService _prodService;

        public AdminController(
            ICategoryService catSerivce,
            IProductService prodService
            )
        {
            _catSerivce = catSerivce;
            _prodService = prodService;
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] AddProductModel request)
        {
            var prod = await _prodService.AddProduct(request);
            return View();
        }

        [HttpGet]
        public IActionResult Products()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ProductsPaging(string keyword, Guid? categoryId, int? page = 1, int? pageSize = 20)
        {
            var paged = await _prodService.Products(keyword, categoryId, page, pageSize);
            return Json(paged);
        }

        [HttpGet]
        public async Task<JsonResult> DeleteProduct([FromQuery] Guid id)
        {
            await _prodService.DeleteProduct(id);
            return Json("Đã xóa một sản phẩm");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(Guid id)
        {
            var product = await _prodService.GetProductAsync(id);
            if(product is null)
            {
                return RedirectToAction("NotFound", "Redirect");
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDetail product)
        {
            await _prodService.UpdateProduct(product.ToProduct());

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult Sales()
        {
            return View("Sales");
        }

        #region API
        [HttpGet]
        public async Task<JsonResult> Categories()
        {
            var categories = await _catSerivce.Categories();

            return Json(categories);
        }
        #endregion
    }
}
