using Mas.Application.ReportServices.Dtos;
using Mas.Core;
using Mas.Core.AppDbContexts;
using Mas.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IAsyncRepository<Invoice> _repository;
        private readonly AppDbContext _context;

        public ReportService(IAsyncRepository<Invoice> repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<ReportRevenue> GetReportRevenueReport(ReportRevenueFilter request)
        {
            string sqlQuery = "sp_report_revenue @startDate,@endDate,@categoryId,@userId,@customerId";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter{ParameterName = "@idChuyenMuc", Value = 188, Direction = System.Data.ParameterDirection.Input},
                new SqlParameter{ParameterName = "@idSotinbai", Value = quantity, Direction = System.Data.ParameterDirection.Input},
                new SqlParameter{ParameterName = "@Loai_HanVanBan", Value = 0, Direction = System.Data.ParameterDirection.Input}
            };
            _context.Database.ExecuteSqlRawAsync()
        }
    }
}
