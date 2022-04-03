using Mas.Application.ManufactureServices.Dtos;
using Mas.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ManufactureServices
{
    public interface IManufactureService
    {
        Task Add(AddManufacture request);

        Task<PagedResult<ManufactureItem>> GetPaged(string keyword, Guid? group, int? page = 1, int? pageSize = 10);
    }
}
