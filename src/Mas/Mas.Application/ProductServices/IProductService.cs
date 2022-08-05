using Mas.Application.ProductServices.Dtos;
using Mas.Common;
using Mas.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.ProductServices
{
    public interface IProductService
    {
        Task<Product> AddProduct(AddProductModel model);

        Task<PagedResult<ProductItem>> Products(string keyword, Guid? category, int? page = 1, int? pageSize = 10);

        Task DeleteProduct(Guid id);

        Task UpdateProduct(Product product);

        Task<ProductDetail> GetProductAsync(Guid id);

        Task<ProductSell> GetProductAsync(string barcode, bool isWholeSale);

        Task<string> ExportProducts(Guid? categoryId);

        Task ImportProducts(IFormFile file);
    }
}
