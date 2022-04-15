using Mas.Application.InventoryServices.Dtos;
using Mas.Common;
using Mas.Core.Entities;
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

        Task<InventoryItemInfo> GetItemInfoAsync(Guid id);

        Task AddDestruction(AddDestruction request);

        Task<PagedResult<DestructionItem>> DestructionsPaging(string keyword, DateTime? start, DateTime? end, int? page, int? pageSize);
    }
}
