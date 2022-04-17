using Mas.Application.ImportServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ImportServices
{
    public class ImportService : IImportService
    {
        private readonly IAsyncRepository<Import> _repository;
        private readonly IAsyncRepository<ImportDetail> _detailRepository;
        private readonly IAsyncRepository<InventoryItem> _inventoryRepository;

        public ImportService(IAsyncRepository<Import> repository,
            IAsyncRepository<ImportDetail> detailRepository,
            IAsyncRepository<InventoryItem> inventoryRepository
            )
        {
            _repository = repository;
            _detailRepository = detailRepository;
            _inventoryRepository = inventoryRepository;
        }
        public async Task AddAsync(AddImportRequest request)
        {
            var import = await _repository.AddAsync(request.ToEntity());
            var items = request.Items.Select(item => item.ToEntity(import.Id));
            await _detailRepository.AddRangeAsync(items);

            var ids = items.Select(c => c.ProductId);
            var inventories = (await _inventoryRepository.FindAllAsync(c => ids.Contains(c.ProductId), new List<string>() { "Product", "Product.Prices" }))
                .ToList();
            
            foreach(var item in items)
            {
                foreach(var inventory in inventories)
                {
                    if(inventory.Product.Id == item.ProductId)
                    {
                        var transfer = inventory.Product.Prices.FirstOrDefault(c => c.UnitId == item.UnitId).TransferQuantity;
                        int plusQuantity = item.Quantity * transfer;
                        inventory.Quantity += plusQuantity;
                        break;
                    }
                }
            }

            await _inventoryRepository.UpdateRangeAsync(inventories);
        }

        public async Task<Import> GetImport(Guid id)
        {
            var entity = await _repository.FindAsync(id, new List<string>() { "Manufacture", "ImportDetails" });

            return entity;
        }

        public async Task<PagedResult<ImportInvoiceItem>> GetPagedResult(string keyword, string startDate, string endDate, int? page = 1, int? pageSize = 10)
        {
            var query = _repository.GetQueryable(new List<string>() { "Manufacture" });
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.SearchParams.Contains(keyword));
            }

            if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var end = DateTime.Parse(endDate).Date.AddDays(1);
                query = query.Where(c => c.CreatedAt < end);
            }

            if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                var start = DateTime.Parse(startDate).Date.AddDays(-1);
                query = query.Where(c => c.CreatedAt > start);
            }

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var start = DateTime.Parse(startDate).Date.AddDays(-1);
                var end = DateTime.Parse(endDate).Date.AddDays(1);
                query = query.Where(c => c.CreatedAt > start && c.CreatedAt < end);
            }

            var paged = await _repository.FindPagedAsync(query, null, page.Value, pageSize.Value);

            return paged.ChangeType(ImportInvoiceItem.FromEntity);

        }
    }
}
