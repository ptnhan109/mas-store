using Mas.Application.ReportServices;
using Mas.Application.ReportServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var data = await _service.GetReportRevenueReport(new ReportRevenueFilter()
            {

            });
            return View();
        }


    }
}
