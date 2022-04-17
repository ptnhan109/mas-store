using Mas.Application.CategoryServices;
using Mas.Application.CategoryServices.Dtos;
using Mas.Application.CustomerGroupServices;
using Mas.Application.CustomerGroupServices.Dtos;
using Mas.Application.CustomerServices;
using Mas.Application.CustomerServices.Dtos;
using Mas.Application.Helper;
using Mas.Application.ImportServices;
using Mas.Application.ImportServices.Dtos;
using Mas.Application.InventoryServices;
using Mas.Application.InventoryServices.Dtos;
using Mas.Application.ManufactureGroupServices;
using Mas.Application.ManufactureGroupServices.Dtos;
using Mas.Application.ManufactureServices;
using Mas.Application.ManufactureServices.Dtos;
using Mas.Application.ProductServices;
using Mas.Application.ProductServices.Dtos;
using Mas.Application.UserServices;
using Mas.Application.UserServices.Dtos;
using Mas.Common;
using Mas.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    //[Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _catSerivce;
        private readonly IProductService _prodService;
        private readonly ICustomerService _cusService;
        private readonly ICustomerGroupService _cusGroupService;
        private readonly IUserService _userService;
        private readonly IManufactureGroupService _manufactureGroupService;
        private readonly IManufactureService _manufactureService;
        private readonly IInventoryService _inventoryService;
        private readonly IImportService _importService;

        public AdminController(
            ICategoryService catSerivce,
            IProductService prodService,
            ICustomerService cusService,
            ICustomerGroupService cusGroupService,
            IUserService userService,
            IManufactureGroupService manufactureGroupService,
            IManufactureService manufactureService,
            IInventoryService inventoryService,
            IImportService importService
            )
        {
            _catSerivce = catSerivce;
            _prodService = prodService;
            _cusService = cusService;
            _cusGroupService = cusGroupService;
            _userService = userService;
            _manufactureGroupService = manufactureGroupService;
            _manufactureService = manufactureService;
            _inventoryService = inventoryService;
            _importService = importService;
        }
        #region PRODUCT
        [Route("quan-tri/them-san-pham")]
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddProductJson([FromBody] AddProductModel request)
        {
            var prod = await _prodService.AddProduct(request);
            var add = new AddInventoryItem(prod.Id, request.CurrentQuantity);
            await _inventoryService.AddInventoryItem(add);
            return Json("Thêm mới hàng hóa thành công");
        }

        [Route("quan-tri/san-pham")]
        [HttpGet]
        public IActionResult Products()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ProductsPaging(string keyword, Guid? categoryId, int? page = 1, int? pageSize = 10)
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetCustomers(string keyword, Guid? group, int? page = 1, int? pageSize = 20)
        {
            var customers = await _cusService.GetCustomers(keyword, group);

            return Json(customers);
        }

        [HttpPost]
        public async Task<JsonResult> AddCustomer([FromBody] AddCustomerRequest request)
        {
            await _cusService.AddCustomer(request);

            return Json("Thêm mới khách hàng thành công.");
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomer(Guid id)
        {
            return Json(await _cusService.GetCustomer(id));
        }
        
        [HttpGet]
        public async Task<JsonResult> DeleteCustomer(Guid id)
        {
            await _cusService.DeleteCustomer(id);

            return Json("Đã xóa khách hàng");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCustomer([FromBody] UpdateCustomerRequest request)
        {
            await _cusService.UpdateCustomer(request);

            return Json("Cập nhật thông tin khách hàng thành công.");
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

        #region USERS
        [Route("quan-tri/nhan-vien")]
        public IActionResult Employees(EnumRole? role)
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetEmployees(EnumRole? role)
        {
            var users = await _userService.GetUsers(role);
            return Json(users.Where(c => !c.Username.ToLower().Equals("admin")));
        }

        [HttpPost]
        public async Task<JsonResult> AddEmployee([FromForm] AddUserRequest request)
        {
            await _userService.AddUser(request);

            return Json("Thêm mới nhân viên thành công.");
        }
        
        [HttpGet]
        public async Task<JsonResult> DeleteEmployee(Guid id)
        {
            await _userService.DeleteUser(id);

            return Json("Xóa nhân viên thành công.");
        }

        [HttpGet]
        public async Task<JsonResult> GetEmployee(Guid id)
        {
            var user = await _userService.GetUser(id);

            return Json(user);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateEmployee([FromForm] UpdateUserRequest request)
        {
            await _userService.UpdateUser(request);

            return Json("Cập nhật nhân viên thành công.");
        }
        #endregion

        #region MANUFACTURE GROUP
        [Route("quan-tri/nhom-nha-cung-cap")]
        public IActionResult ManufactureGroups()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<JsonResult> GetManufactureGroups()
        {
            var items = await _manufactureGroupService.GetAllAsync();
            return Json(items);
        }


        [HttpPost]
        public async Task<JsonResult> AddManufactureGroup([FromBody] AddManufactureGroup request)
        {
            await _manufactureGroupService.AddAsync(request);

            return Json("Thêm mới nhóm nhà cung cấp thành công.");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateManufactureGroup([FromBody] UpdateManufactureGroup request)
        {
            await _manufactureGroupService.UpdateAsync(request);

            return Json("Cập nhật nhóm nhà cung cấp thành công.");
        }

        [HttpGet]
        public async Task<JsonResult> DeleteManufactureGroup(Guid id)
        {
            await _manufactureGroupService.DeleteAsync(id);

            return Json("Xóa nhóm nhà cung cấp thành công.");
        }
        #endregion

        #region MANUFACTURE
        [Route("quan-tri/them-nha-cung-cap")]
        [HttpGet]
        public async Task<IActionResult> AddManufacture()
        {
            var manufactureGroups = await _manufactureGroupService.GetAllAsync();
            return View(manufactureGroups);
        }

        [HttpPost]
        public async Task<IActionResult> CreateManufacture([FromForm] AddManufacture request)
        {
            await _manufactureService.Add(request);

            return RedirectToAction("Manufactures");
        }

        [Route("quan-tri/nha-cung-cap")]
        [HttpGet]
        public async Task<IActionResult> Manufactures()
        {
            await Task.Yield();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetManufactures(string keyword, Guid? group, int? page = 1, int? pageSize = 10)
        {
            var result = await _manufactureService.GetPaged(keyword, group, page, pageSize);

            return Json(result);
        }

        [Route("quan-tri/nha-cung-cap-chi-tiet")]
        [HttpGet]
        public async Task<IActionResult> ManufactureById(Guid id)
        {
            var result = await _manufactureService.GetById(id);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateManufacture([FromForm] UpdateManufacture request)
        {
            await _manufactureService.UpdateAsync(request);

            return RedirectToAction("Manufactures");
        }
        #endregion

        #region INVENTORY
        [AllowAnonymous]
        [Route("quan-tri/quan-ly-kho")]
        public async Task<IActionResult> Inventories(string keyword, Guid? groupId, bool? limit,int? page = 1, int? pageSize = 10)
        {
            await Task.Yield();
            return View();
        }

        [Route("quan-tri/hang-ton")]
        public async Task<IActionResult> ProductInventory()
        {
            var dashboard = await _inventoryService.Dashboard();
            return View(dashboard);
        }

        [HttpGet]
        public async Task<JsonResult> SearchInventories(string keyword, Guid? categoryId, int? isPassQuota, int? page = 1, int? pageSize = 10)
        {
            if(isPassQuota != null)
            {
                bool pass = isPassQuota.Value == 1 ? true : false;
                var items = await _inventoryService.GetInventories(keyword, categoryId, pass, page, pageSize);
                
                return Json(items);
            }

            var result = await _inventoryService.GetInventories(keyword, categoryId, null, page, pageSize);

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> InventoryInfo(Guid id)
        {
            var data = await _inventoryService.GetItemInfoAsync(id);

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> ImportInventory([FromBody] AddImportRequest request)
        {
            request.CreatedBy = HttpContext.User.FindFirst("Name").Value.ToString();
            await _importService.AddAsync(request);
            return Json(true);
        }
        #endregion

        #region DESTRUCTION
        [Route("quan-tri/phieu-huy")]
        public async Task<IActionResult> Destructions()
        {
            await Task.Yield();

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetDestructionPaging(string keyword, string startDate, string endDate, int? page = 1, int? pageSize = 10)
        {
            if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                var start = DateTime.Parse(startDate);
                return Json(await _inventoryService.DestructionsPaging(keyword, start, null, page, pageSize));
            }

            if (!string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(startDate))
            {
                var end = DateTime.Parse(endDate);
                return Json(await _inventoryService.DestructionsPaging(keyword, null, end, page, pageSize));
            }

            if(!string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(startDate))
            {
                var start = DateTime.Parse(startDate);
                var end = DateTime.Parse(endDate);
                return Json(await _inventoryService.DestructionsPaging(keyword, start, end, page, pageSize));
            }

            if(string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(startDate))
            {
                return Json(await _inventoryService.DestructionsPaging(keyword, null, null, page, pageSize));
            }

            return default;

        }

        [Route("quan-tri/tao-phieu-huy-hang")]
        public async Task<IActionResult> CreateDestruction()
        {
            await Task.Yield();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddDestruction([FromBody] AddDestruction request)
        {
            await _inventoryService.AddDestruction(request);

            return Json("Hủy hàng thành công");
        }

        [HttpGet]
        public async Task<JsonResult> PrintDestruction(Guid id)
        {
            var destruction = await _inventoryService.GetByIdAsync(id);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "destruction.html");
            string templates = System.IO.File.ReadAllText(path);
            templates = templates.Replace("{createdBy}", destruction.CreatedBy);
            templates = templates.Replace("{cancelDate}", destruction.CreatedAt.ToString("dd/MM/yyyy"));
            var content = new StringBuilder();
            var items = destruction.DestructionDetails.ToList();
            int quantity = 0;
            for(int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                content.Append(@"<tr><td class='text-center-item'>");
                content.Append((i + 1));
                content.Append(@"</td><td class='text-left item'>");
                content.Append(item.Name);
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.CurrentImport.ToCurrencyFormat());
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.Quantity);
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.Amount.ToCurrencyFormat());
                content.Append("</td></tr>");
                quantity += item.Quantity;
            }

            templates = templates.Replace("{content}", content.ToString());
            templates = templates.Replace("{amount}", destruction.Amount.ToCurrencyFormat());
            templates = templates.Replace("{quantity}", quantity.ToString());

            return Json(templates);
        }
        #endregion

        #region IMPORT
        [Route("quan-tri/nhap-hang")]
        public async Task<IActionResult> Imports()
        {
            await Task.Yield();
            return View();
        }

        [Route("quan-tri/them-phieu-nhap")]
        public async Task<IActionResult> AddImportInvoice()
        {
            await Task.Yield();
            return View();
        }

        public async Task<JsonResult> ImportInvoices(string keyword, string startDate, string endDate, int? page = 1, int? pageSize = 10)
        {
            var paged = await _importService.GetPagedResult(keyword, startDate, endDate, page, pageSize);

            return Json(paged);
        }

        [HttpGet]
        public async Task<JsonResult> PrintImportInvoice(Guid id)
        {
            var import = await _importService.GetImport(id);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "import.html");
            string templates = System.IO.File.ReadAllText(path);
            templates = templates.Replace("{manufacture-name}", import.Manufacture.Name);
            templates = templates.Replace("{manufacture-address}", import.Manufacture.Address);
            templates = templates.Replace("{orderDate}", import.CreatedAt.ToString("dd/MM/yyyy"));
            var content = new StringBuilder();
            var items = import.ImportDetails.ToList();
            int quantity = 0;
            double discount = 0;
            double total = 0;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                content.Append(@"<tr><td class='text-center-item'>");
                content.Append((i + 1));
                content.Append(@"</td><td class='text-left item'>");
                content.Append(item.Name);
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.Quantity);
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.CurrentPrice.ToCurrencyFormat());
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.Discount.ToCurrencyFormat());
                content.Append(@"</td><td class='text-center item'>");
                content.Append(item.Amount.ToCurrencyFormat());
                content.Append("</td></tr>");
                quantity += item.Quantity;
                discount += item.Discount;
                total += item.Quantity * item.CurrentPrice;
            }

            templates = templates.Replace("{content}", content.ToString());
            templates = templates.Replace("{beforeDiscount}", total.ToCurrencyFormat());
            templates = templates.Replace("{discount}", discount.ToString());
            templates = templates.Replace("{checkout}", import.Amount.ToString());

            return Json(templates);
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

        [HttpGet]
        public async Task<JsonResult> SearchProduct(string keyword)
        {
            var items = await _prodService.Products(keyword, null, 1, 10);

            return Json(items);
        }

        [HttpGet]
        public async Task<JsonResult> ExportProducts(Guid? cateId)
        {
            string result = await _prodService.ExportProducts(cateId);
            return Json($"danh-sach-hang-hoa-{result}");
        }
        #endregion

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Test()
        {
            var s = await _prodService.ExportProducts(null);
            return Json("a");
        }
    }
}
