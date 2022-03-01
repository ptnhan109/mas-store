using Mas.Application.CategoryServices;
using Mas.Application.ProductServices;
using Mas.Application.ProductServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    [Authorize]
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
