using Mas.Application.InvoiceServices;
using Mas.Application.InvoiceServices.Dtos;
using Mas.Application.ProductServices;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    //[Authorize(Policy = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IProductService _service;
        private readonly IInvoiceService _invoiceService;

        public EmployeeController(IProductService service, IInvoiceService invoiceService)
        {
            _service = service;
            _invoiceService = invoiceService;
        }
        [Route("ban-hang")]
        public IActionResult Sales()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProduct(string barcode, bool isWholeSale)
        {
            var prod = await _service.GetProductAsync(barcode, isWholeSale);

            return Json(prod);
        }

        [HttpPost]
        public JsonResult PrintInvoices([FromBody] AddInvoiceRequest request)
        {
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
                items.Append((i + 1).ToString());
                items.Append(@"</td><td class='text-left item'>");
                items.Append(invoice.Name);
                items.Append("<br />");
                items.Append(invoice.Quantity);
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
            await _invoiceService.AddAsync(request);
            return Json("Thanh toán thành công");
        }
    }
}
