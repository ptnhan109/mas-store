using Mas.Application.ReportServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ReportServices
{
    public interface IReportService
    {
        Task<ReportRevenue> GetReportRevenueReport(ReportRevenueFilter request);
    }
}
