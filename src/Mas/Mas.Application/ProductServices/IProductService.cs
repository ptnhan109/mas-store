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

        Task UpdateProduct(UpdateProductRequest request);

        Task<ProductDetail> GetProductAsync(Guid id);

        Task<ProductSell> GetProductAsync(string barcode, bool isWholeSale, Guid? id);

        Task<string> ExportProducts(Guid? categoryId);

        Task ImportProducts(IFormFile file);

        Task<ProductUpdatePrice> GetProductUpdate(Guid id);

        Task UpdateProductPrice(ProductUpdatePrice price);

        Task<IEnumerable<PrintPrice>> GetPrintPrices(IEnumerable<Guid> ids);
    }
}
