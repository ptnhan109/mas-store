using Mas.Application.InvoiceServices;
using Mas.Application.InvoiceServices.Dtos;
using Mas.Application.ProductServices;
using Mas.Application.ProductServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    //[Authorize(Policy = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IProductService _service;
        private readonly IInvoiceService _invoiceService;
        private readonly IHttpContextAccessor _httpContext;

        public EmployeeController(IProductService service, IInvoiceService invoiceService, IHttpContextAccessor httpContext)
        {
            _service = service;
            _invoiceService = invoiceService;
            _httpContext = httpContext;
        }
        [Route("ban-hang")]
        public IActionResult Sales()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProduct(string barcode, bool isWholeSale, Guid? id)
        {
            var prod = await _service.GetProductAsync(barcode, isWholeSale,id);
            if(prod is null)
            {
                return Json(false);
            }
            return Json(prod);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductUpdatePrice(Guid id)
        {
            var prod = await _service.GetProductUpdate(id);
            if(prod is null)
            {
                return Json(false);
            }

            return Json(prod);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProductPrice([FromBody] ProductUpdatePrice request)
        {
            await _service.UpdateProductPrice(request);
            return Json(true);
        }

        [HttpPost]
        public JsonResult PrintInvoices([FromBody] AddInvoiceRequest request)
        {
            if (!request.InvoiceDetails.Any())
            {
                return default;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "invoices.html");
            string templates = System.IO.File.ReadAllText(path);
            templates = templates.Replace("{customerName}", request.CustomerName);
            templates = templates.Replace("{orderDate}", DateTime.Now.ToString("dd-MM-yyyy"));
            templates = templates.Replace("{customerAddress}", "");
            templates = templates.Replace("{note}", request.Note);
            StringBuilder items = new StringBuilder();
            double beforeDiscount = 0;

            for (int i = 0; i < request.InvoiceDetails.Count; i++)
            {
                var invoice = request.InvoiceDetails[i];
                var afterDiscount = invoice.CurrentPrice - invoice.Discount;
                items.Append(@"<tr><td class='text-center item'>");
                items.Append((invoice.Quantity).ToString());
                items.Append(@"</td><td class='text-left item'>");
                items.Append(invoice.Name);
                items.Append(@"</td><td class='text-center item'>");
                items.Append(invoice.CurrentPrice.ToCurrencyFormat());
                items.Append(@"</td><td class='text-center item'>");
                items.Append(invoice.Discount.ToCurrencyFormat());
                items.Append(@"</td><td class='text-right item'>");
                items.Append((afterDiscount * invoice.Quantity).ToCurrencyFormat());
                items.Append("</td></tr>");
                beforeDiscount += afterDiscount * invoice.Quantity;
            }
            templates = templates.Replace("{content}", items.ToString());
            templates = templates.Replace("{beforeDiscount}", beforeDiscount.ToCurrencyFormat());
            templates = templates.Replace("{discount}", request.Discount.ToCurrencyFormat());
            templates = templates.Replace("{checkout}", (beforeDiscount - request.Discount).ToCurrencyFormat());

            return Json(templates);
        }

        [HttpPost]
        public async Task<JsonResult> AddInvoice([FromBody] AddInvoiceRequest request)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            request.EmployeeId = Guid.Parse(userId);
            await _invoiceService.AddAsync(request);
            return Json("Thanh toán thành công");
        }
    }
}
