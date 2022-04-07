using Mas.Application.InventoryServices.Dtos;
using Mas.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.InventoryServices
{
    public interface IInventoryService
    {
        Task AddInventoryItem(AddInventoryItem item);

        Task<PagedResult<InventoryListItem>> GetInventories(string keyword, Guid? categoryId, bool? isPassQuota, int? page = 1, int? pageSize = 10);

        Task<InventoryDashboard> Dashboard();
    }
}
