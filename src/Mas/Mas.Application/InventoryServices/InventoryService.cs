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
        private readonly IAsyncRepository<Destruction> _desRepo;
        private readonly IAsyncRepository<DestructionDetail> _desDetailRepo;
        private readonly IAsyncRepository<InventoryItem> _invtyItemRepo;

        public InventoryService(
            IAsyncRepository<InventoryItem> repository,
            IAsyncRepository<Destruction> desRepo,
            IAsyncRepository<DestructionDetail> desDetailRepo,
            IAsyncRepository<InventoryItem> invtyItemRepo
            )
        {
            _repository = repository;
            _desRepo = desRepo;
            _desDetailRepo = desDetailRepo;
            _invtyItemRepo = invtyItemRepo;
        }


        public async Task AddInventoryItem(AddInventoryItem item)
        {
            await _repository.AddAsync(item.ToEntity());
        }

        public async Task<InventoryDashboard> Dashboard()
        {
            var query = _repository.GetQueryable(new List<string>() { "Product", "Product.Prices", "Product.Category" });
            int below = await query.CountAsync(c => c.Quantity <= c.Product.InventoryLimit);
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
                query = query.Where(c => c.Product.SearchParams.Contains(keyword));
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


        #region DESTRUCTION
        public async Task AddDestruction(AddDestruction request)
        {
            var inserted = await _desRepo.AddAsync(request.ToEntity());
            var details = request.Items.Select(de => de.ToEntity(inserted.Id));
            if (inserted != null)
            {
                await _desDetailRepo.AddRangeAsync(details);
            }

            var ids = details.Select(c => c.ProductId);

            // subtract inventory items
            var inventories = (await _invtyItemRepo.FindAllAsync(c => ids.Contains(c.ProductId), new List<string>() { "Product", "Product.Prices" }))
                .ToList();
            foreach(var inventory in inventories)
            {
                var destruction = details?.FirstOrDefault(detail => detail.ProductId == inventory.ProductId);
                var transfer = inventory.Product.Prices.FirstOrDefault(c => c.UnitId == destruction.UnitId).TransferQuantity;
                inventory.Quantity = inventory.Quantity - destruction.Quantity * transfer;
            }

            await _invtyItemRepo.UpdateRangeAsync(inventories);
        }

        public async Task<PagedResult<DestructionItem>> DestructionsPaging(string keyword, DateTime? start, DateTime? end, int? page, int? pageSize)
        {
            var query = _desRepo.GetQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.SearchParams.Contains(keyword));
            }

            if(start == null && end != null)
            {
                var endDate = end.Value.Date.AddDays(1);
                query = query.Where(c => c.CreatedAt < endDate);
            }

            if(start != null && end == null)
            {
                var startDate = end.Value.Date.AddDays(-1);
                query = query.Where(c => c.CreatedAt > startDate);
            }

            if(start != null && end != null)
            {
                var startDate = start.Value.AddDays(-1);
                var endDate = end.Value.AddDays(1);
                query = query.Where(c => c.CreatedAt > start && c.CreatedAt < end);
            }

            var paged = await _desRepo.FindPagedAsync(query, null, page.Value, pageSize.Value);

            return paged.ChangeType(DestructionItem.FromEntity);
        }

        #endregion
    }
}
