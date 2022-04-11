using Mas.Application.InvoiceServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.InvoiceServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IAsyncRepository<Invoice> _repository;
        private readonly IAsyncRepository<InvoiceDetail> _detailRepository;
        private readonly IAsyncRepository<Product> _prodRepository;
        private readonly IAsyncRepository<InventoryItem> _inventoryRepository;

        public InvoiceService(
            IAsyncRepository<Invoice> repository, 
            IAsyncRepository<InvoiceDetail> detailRepository, 
            IAsyncRepository<Product> prodService,
            IAsyncRepository<InventoryItem> inventoryRepository)
        {
            _repository = repository;
            _detailRepository = detailRepository;
            _prodRepository = prodService;
            _inventoryRepository = inventoryRepository;
        }
        public async Task AddAsync(AddInvoiceRequest request)
        {
            // add invoice
            var inserted = await _repository.AddAsync(request.ToEntity());
            var details = request.InvoiceDetails.Select(c => c.ToEntity(inserted.Id)).ToList();
            var products = new List<Product>();
            foreach(var detail in details)
            {
                var prod = await _prodRepository.FindAsync(detail.ProductId,new List<string>() { "Prices"});
                detail.CurrentImport = prod.Prices.FirstOrDefault(c => c.BarCode.Equals(detail.BarCode)).ImportPrice;
                detail.Profit = Math.Round((detail.CurrentPrice - detail.CurrentImport - detail.Discount) * detail.Quantity);
                detail.Name = prod.Name;
                products.Add(prod);
            }
            await _detailRepository.AddRangeAsync(details);

            // subtract quantity in inventory
            var ids = request.InvoiceDetails.Select(c => c.ProductId);
            var inventories = (await _inventoryRepository.FindAllAsync(c => ids.Contains(c.ProductId), null)).ToList();
            inventories.ForEach(inventory =>
            {
                var quantity = details?.FirstOrDefault(detail => detail.ProductId == inventory.ProductId);
                var tranfers = products.FirstOrDefault(prod => prod.Id == inventory.ProductId)
                    .Prices.FirstOrDefault(price => price.UnitId == quantity.UnitId).TransferQuantity;
                inventory.Quantity = inventory.Quantity - quantity.Quantity * tranfers;
            });

            await _inventoryRepository.UpdateRangeAsync(inventories);

        }
    }
}
