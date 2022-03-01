using Mas.Application.ProductServices.Dtos;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IAsyncRepository<Product> _repository;

        public ProductService(IAsyncRepository<Product> repository)
        {
            _repository = repository;
        }
        public async Task<Product> AddProduct(AddProductModel model)
        {
            var product = model.ToProduct();
            if(model.ParentUnitId != null)
            {
                var tranfers = model.ToPrice(product.Id);
                product.Price = new List<Price>() { tranfers };
            }

            return await _repository.AddAsync(product);
        }
    }
}
