using Mas.Application.CategoryServices;
using Mas.Application.CategoryServices.Dtos;
using Mas.Application.CustomerGroupServices;
using Mas.Application.CustomerGroupServices.Dtos;
using Mas.Application.CustomerServices;
using Mas.Application.Helper;
using Mas.Application.ProductServices;
using Mas.Application.ProductServices.Dtos;
using Mas.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _catSerivce;
        private readonly IProductService _prodService;
        private readonly ICustomerService _cusService;
        private readonly ICustomerGroupService _cusGroupService;

        public AdminController(
            ICategoryService catSerivce,
            IProductService prodService,
            ICustomerService cusService,
            ICustomerGroupService cusGroupService
            )
        {
            _catSerivce = catSerivce;
            _prodService = prodService;
            _cusService = cusService;
            _cusGroupService = cusGroupService;
        }
        #region PRODUCT
        [Route("quan-tri/them-san-pham")]
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<JsonResult> AddProduct([FromBody] AddProductModel request)
        {
            var prod = await _prodService.AddProduct(request);
            return Json("Thêm mới hàng hóa thành công");
        }

        [Route("quan-tri/san-pham")]
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

        [Route("quan-tri/cap-nhat-san-pham")]
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(Guid id)
        {
            var product = await _prodService.GetProductAsync(id);
            if (product is null)
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

        #endregion

        #region CATEGORY

        [Route("quan-tri/danh-muc")]
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            await Task.Yield();
            return View();
        }

        #endregion

        #region BARCODE
        [Route("quan-tri/ma-vach")]
        public async Task<IActionResult> BarCode()
        {
            await Task.Yield();
            return View();
        }
        #endregion

        #region CUSTOMER
        [Route("quan-tri/khach-hang")]
        [HttpGet]
        public IActionResult Customers()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetCustomers(string keyword, Guid group)
        {
            var customers = await _cusService.GetCustomers(keyword, group);

            return Json(customers);
        }

        [Route("quan-tri/nhom-khach-hang")]
        [HttpGet]
        public IActionResult CustomerGroups()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomerGroups()
        {
            var groups = await _cusGroupService.CustomerGroups();

            return Json(groups);
        }

        [HttpPost]
        public async Task<JsonResult> AddCustomerGroup([FromBody] AddCustomerGroup request)
        {
            await _cusGroupService.AddAsync(request);
            return Json("Thêm mới nhóm khách hàng thành công");
        }

        [HttpGet]
        public async Task<JsonResult> DeleteCustomerGroup(Guid id)
        {
            await _cusGroupService.DeleteAsync(id);
            return Json("Xóa nhóm khách hàng thành công");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCustomerGroup([FromBody] UpdateCustomerGroup request)
        {
            await _cusGroupService.UpdateAsync(request);
            return Json("Cập nhật nhóm người dùng thành công");
        }
        #endregion
        #region API
        [HttpGet]
        public async Task<JsonResult> Categories()
        {
            var categories = await _catSerivce.Categories();

            return Json(categories);
        }

        [HttpPost]
        public async Task<JsonResult> AddCategory([FromBody] AddCatRequest request)
        {
            await _catSerivce.AddAsync(request);
            return Json("Thêm mới danh mục thành công");
        }

        [HttpPut]
        public async Task<JsonResult> UpdateCategory([FromBody] UpdateCatRequest request)
        {
            await _catSerivce.UpdateAsync(request);
            return Json("Cập nhật danh mục thành công.");
        }

        [HttpGet]
        public async Task<JsonResult> DeleteCategory(Guid id)
        {
            await _catSerivce.DeleteAsync(id);
            return Json("Xóa danh mục thành công.");
        }

        [HttpGet]
        public async Task<JsonResult> GenerateBarcode(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(false);
            }
            await BarCodeHelper.GenerateBarCode(data);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> PrintBarcode([FromBody] PrintBarcode request)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "print-barcode.html");
            string templates = System.IO.File.ReadAllText(path);
            templates = templates.Replace("{name}", request.Name);
            StringBuilder items = new StringBuilder();
            for (int i = 0; i < request.Quantity; i++)
            {
                items.Append(@"<img src='/assets/barcode/barcode.png' />");
            }

            templates = templates.Replace("{content}", items.ToString());

            await Task.Yield();
            return Json(templates);
        }
        #endregion
    }
}
