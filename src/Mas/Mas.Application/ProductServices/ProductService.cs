using Mas.Application.ProductServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IAsyncRepository<Product> _repository;
        private readonly IAsyncRepository<Price> _priceRepository;

        public ProductService(IAsyncRepository<Product> repository, IAsyncRepository<Price> priceRepository)
        {
            _repository = repository;
            _priceRepository = priceRepository;
        }
        public async Task<Product> AddProduct(AddProductModel model)
        {
            var product = model.ToProduct();

            return await _repository.AddAsync(product);
        }

        public async Task DeleteProduct(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ProductDetail> GetProductAsync(Guid id)
        {
            var product = await _repository.FindAsync(id, new List<string>() { "Category", "Prices" });

            if (product is null)
            {
                return default;
            }

            return new ProductDetail(product);
        }

        public async Task<ProductSell> GetProductAsync(string barcode)
        {
            Expression<Func<Product, bool>> expression = c => c.BarCode.Equals(barcode);
            var prod = await _repository.FindAsync(expression, new List<string>() { "Category", "Prices" });
            if(prod is null)
            {
                Expression<Func<Price, bool>> expPrice = c => c.BarCode.Equals(barcode);

                var price = await _priceRepository.FindAsync(expPrice, new List<string>() { "Product", "Product.Prices" });
                if(price is null)
                {
                    return default;
                }
                var prodParent = price.Product;
                prodParent.BarCode = price.BarCode;
                var prices = prodParent.Prices.ToList();
                prices.ForEach(price =>
                {
                    if(price.BarCode == barcode)
                    {
                        price.IsDefault = true;
                    }
                    else
                    {
                        price.IsDefault = false;
                    }
                });

                prodParent.Prices = prices;

                return new ProductSell(prodParent);
            }

            return new ProductSell(prod);
        }

        public async Task<PagedResult<ProductItem>> Products(string keyword, Guid? category, int? page = 1, int? pageSize = 10)
        {
            var query = _repository.GetQueryable(new List<string>() { "Category", "Prices" });
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Name.Contains(keyword) || c.BarCode.Contains(keyword));
            }

            if (category != null)
            {
                query = query.Where(c => c.CategoryId == category.Value);
            }

            var paged = await _repository.FindPagedAsync(query, null, page.Value, pageSize.Value);

            return paged.ChangeType(ProductItem.FromEntity);
        }

        public async Task UpdateProduct(Product product)
        {
            await _repository.UpdateAsync(product);
            await _priceRepository.DeleteRangeAsync(c => c.ProductId == product.Id);
            await _priceRepository.AddRangeAsync(product.Prices);
        }
    }
}
