using Mas.Application.InventoryServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.InventoryServices
{
    public class InventoryService : IInventoryService
    {
        private readonly IAsyncRepository<InventoryItem> _repository;

        public InventoryService(IAsyncRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public async Task AddInventoryItem(AddInventoryItem item)
        {
            await _repository.AddAsync(item.ToEntity());
        }

        public async Task<InventoryDashboard> Dashboard()
        {
            var query = _repository.GetQueryable(new List<string>() { "Product", "Product.Prices", "Product.Category" });
            int below = await query.CountAsync(c => c.Quantity < c.Product.InventoryLimit);
            int total = await query.CountAsync();

            return new InventoryDashboard()
            {
                BelowQuota = below,
                TotalMoney = 0,
                TotalProductQuantity = total
            };
        }

        public async Task<PagedResult<InventoryListItem>> GetInventories(string keyword, Guid? categoryId, bool? isPassQuota, int? page = 1, int? pageSize = 10)
        {
            var query = _repository.GetQueryable(new List<string>() { "Product", "Product.Prices" });

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Product.Name.Contains(keyword));
            }

            if (categoryId != null)
            {
                query = query.Where(c => c.Product.CategoryId == categoryId.Value);
            }

            if (isPassQuota != null)
            {
                if (isPassQuota.Value)
                {
                    query = query.Where(c => c.Quantity > c.Product.InventoryLimit);
                }
                else
                {
                    query = query.Where(c => c.Quantity <= c.Product.InventoryLimit);
                }
            }

            var paged = await _repository.FindPagedAsync(query, null, page.Value, pageSize.Value);

            return paged.ChangeType(InventoryListItem.FromEntity);
        }

        public async Task<InventoryItemInfo> GetItemInfoAsync(Guid id)
        {
            var entity = await _repository.FindAsync(id, new List<string>() { "Product", "Product.Prices", "Product.Category" });

            return new InventoryItemInfo(entity);
        }
    }
}
