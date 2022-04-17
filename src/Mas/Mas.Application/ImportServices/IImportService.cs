using Mas.Application.ImportServices.Dtos;
using Mas.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ImportServices
{
    public interface IImportService
    {
        Task AddAsync(AddImportRequest request);

        Task<PagedResult<ImportInvoiceItem>> GetPagedResult(string keyword, string startDate, string endDate, int? page = 1, int? pageSize = 10); 
    }
}
