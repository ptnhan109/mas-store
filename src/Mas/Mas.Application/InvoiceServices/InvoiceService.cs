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

        public InvoiceService(
            IAsyncRepository<Invoice> repository, 
            IAsyncRepository<InvoiceDetail> detailRepository, 
            IAsyncRepository<Product> prodService)
        {
            _repository = repository;
            _detailRepository = detailRepository;
            _prodRepository = prodService;
        }
        public async Task AddAsync(AddInvoiceRequest request)
        {
            var inserted = await _repository.AddAsync(request.ToEntity());
            var details = request.InvoiceDetails.Select(c => c.ToEntity(inserted.Id)).ToList();
            foreach(var detail in details)
            {
                var prod = await _prodRepository.FindAsync(detail.ProductId,new List<string>() { "Prices"});
                detail.CurrentImport = prod.Prices.FirstOrDefault(c => c.BarCode.Equals(detail.BarCode)).ImportPrice;
                detail.Profit = Math.Round((detail.CurrentPrice - detail.CurrentImport - detail.Discount) * detail.Quantity);
                detail.Name = prod.Name;
            }

            await _detailRepository.AddRangeAsync(details);
        }
    }
}
