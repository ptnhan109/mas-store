using Mas.Application.ReportServices.Dtos;
using Mas.Core;
using Mas.Core.AppDbContexts;
using Mas.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Mas.Core.Contants;
using OfficeOpenXml;
using System.IO;

namespace Mas.Application.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IAsyncRepository<Invoice> _repository;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ReportService(IAsyncRepository<Invoice> repository,
            AppDbContext context,
            IConfiguration configuration)
        {
            _repository = repository;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> ExportReportRevenueReport(ReportRevenueFilter request)
        {
            var items = await GetReportRevenueReport(request);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "report-revenue.xlsx");
            string fileName = $"{Guid.NewGuid()}.xlsx";
            string newFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", $"bao-cao-doanh-thu-{fileName}");
            var file = new FileInfo(newFile);
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
            {
                var sheet = excelPackage.Workbook.Worksheets.First();
                if(request.Start != null && request.End != null)
                {

                    sheet.Cells["A5"].Value = $"{request.Start.Value.ToString("dd-MM-yyyy hh:mm")} - {request.End.Value.ToString("dd-MM-yyyy hh:mm")}";
                }
                else
                {
                    sheet.Cells["A5"].Value = string.Empty;
                }
                var startRow = 8;
                int index = 0;
                foreach(var item in items)
                {
                    sheet.Cells[$"A{startRow}"].Value = index;
                    sheet.Cells[$"B{startRow}"].Value = item.ProductName;
                    sheet.Cells[$"C{startRow}"].Value = item.Quantity;
                    sheet.Cells[$"D{startRow}"].Value = item.ImportPrice;
                    sheet.Cells[$"E{startRow}"].Value = item.SumImportPrice;
                    sheet.Cells[$"F{startRow}"].Value = item.SellPrice;
                    sheet.Cells[$"G{startRow}"].Value = item.SumSellPrice;
                    sheet.Cells[$"H{startRow}"].Value = item.Profit;
                    sheet.Cells[$"I{startRow}"].Value = item.Discount;
                    sheet.Cells[$"J{startRow}"].Value = item.Unit;
                    sheet.Cells[$"K{startRow}"].Value = item.Reason;
                    startRow++;
                    index++;

                }

                excelPackage.SaveAs(file);
                return fileName;
            }
        }

        public async Task<IEnumerable<ReportRevenueDto>> GetReportRevenueReport(ReportRevenueFilter request)
        {
            
            var connectionString = _configuration.GetSection("ConnectionStrings:Default").Value;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = (await connection.QueryAsync<ReportRevenueDto>("sp_report_revenue", new
                {
                    startDate = request.Start,
                    endDate = request.End,
                    categoryId = request.CategoryId,
                    userId = request.EmployeeId,
                }, null, null, CommandType.StoredProcedure)).ToList();

                var total = new ReportRevenueDto()
                {
                    Discount = data.Sum(c => c.Discount),
                    Profit = data.Sum(c => c.Profit),
                    SumImportPrice = data.Sum(c => c.SumImportPrice),
                    SumSellPrice = data.Sum(C => C.SumSellPrice),
                    ImportPrice = 0,
                    Reason = string.Empty,
                    Quantity = 0,
                    SellPrice = 0,
                    Unit = string.Empty,
                    Category = string.Empty
                };

                data.Insert(0, total);

                return data;
            }
            
        }
    }
}
