﻿using Mas.Application.ReportServices.Dtos;
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
        public async Task<ReportRevenue> GetReportRevenueReport(ReportRevenueFilter request)
        {
            
            var connectionString = _configuration.GetSection("ConnectionStrings:Default").Value;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = await connection.QueryAsync<ReportRevenueDto>("sp_report_revenue",new
                {
                    startDate = request.Start,
                    endDate = request.End,
                    categoryId = request.CategoryId,
                    userId = request.EmployeeId,
                },null,null,CommandType.StoredProcedure);

                var result = new ReportRevenue()
                {
                    Start = request.Start,
                    End = request.End,
                    Items = data,
                    SumDiscount = data.Sum(c => c.Discount),
                    SumImport = data.Sum(c => c.ImportPrice),
                    SumProfit = data.Sum(c => c.Profit),
                    SumSell = data.Sum(c => c.SellPrice)
                };
                connection.Close();
                return result;
            }
            
        }
    }
}
