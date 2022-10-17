using Mas.Application.ReportServices;
using Mas.Application.ReportServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mas.Web.Controllers
{
    [Authorize(Policy = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IReportService _service;

        public DashboardController(IReportService service)
        {
            _service = service;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("bao-cao/doanh-thu")]
        public async Task<IActionResult> ReportRevenue()
        {
            
            return View();
        }

        public async Task<JsonResult> ReportRevenueJson([FromQuery] ReportRevenueFilter request)
        {
            var data = await _service.GetReportRevenueReport(request);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> ExportReportRevenue([FromQuery] ReportRevenueFilter request)
        {
            string path= await _service.ExportReportRevenueReport(request);
            return Json($"bao-cao-doanh-thu-{path}");
        }

    }
}
