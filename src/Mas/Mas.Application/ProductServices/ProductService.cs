using Mas.Application.ProductServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Contants;
using Mas.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IAsyncRepository<Category> _catRepository;
        private readonly IAsyncRepository<InventoryItem> _inventoryRepository;

        public ProductService(IAsyncRepository<Product> repository,
            IAsyncRepository<Price> priceRepository,
            IAsyncRepository<Category> catRepository,
            IAsyncRepository<InventoryItem> inventoryRepository)
        {
            _repository = repository;
            _priceRepository = priceRepository;
            _catRepository = catRepository;
            _inventoryRepository = inventoryRepository;
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
         => new ProductDetail(await GetById(id));

        public async Task<ProductSell> GetProductAsync(string barcode, bool isWholeSale, Guid? id)
        {
            Expression<Func<Product, bool>> expression = c => c.BarCode.Equals(barcode) || c.Id.Equals(id);
            var prod = await _repository.FindAsync(expression, new List<string>() { "Category", "Prices" });
            if (prod is null)
            {
                Expression<Func<Price, bool>> expPrice = c => c.BarCode.Equals(barcode);

                var price = await _priceRepository.FindAsync(expPrice, new List<string>() { "Product", "Product.Prices" });
                if (price is null)
                {
                    return default;
                }
                var prodParent = price.Product;
                prodParent.BarCode = price.BarCode;
                var prices = prodParent.Prices.ToList();
                prices.ForEach(price =>
                {
                    if (price.BarCode == barcode)
                    {
                        price.IsDefault = true;
                    }
                    else
                    {
                        price.IsDefault = false;
                    }
                });

                prodParent.Prices = prices;

                return new ProductSell(prodParent, isWholeSale);
            }

            return new ProductSell(prod, isWholeSale);
        }

        public async Task<PagedResult<ProductItem>> Products(string keyword, Guid? category, int? page = 1, int? pageSize = 10)
        {
            var isNumeric = int.TryParse(keyword, out _);
            if (isNumeric)
            {
                return new PagedResult<ProductItem>();
            }
            var query = _repository.GetQueryable(new List<string>() { "Category", "Prices" });
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.SearchParams.Contains(keyword.ToLower()) || c.BarCode.Contains(keyword));
            }

            if (category != null)
            {
                query = query.Where(c => c.CategoryId == category.Value);
            }

            var paged = await _repository.FindPagedAsync(query, null, page.Value, pageSize.Value);

            return paged.ChangeType(ProductItem.FromEntity);
        }

        public async Task UpdateProduct(UpdateProductRequest request)
        {
            var product = request.ToProduct();
            var prices = product.Prices;
            product.Prices = null;
            await _repository.UpdateAsync(product);
            await _priceRepository.DeleteRangeAsync(c => c.ProductId == product.Id);
            await _priceRepository.AddRangeAsync(prices);
        }

        public async Task<string> ExportProducts(Guid? categoryId)
        {
            var query = _repository.GetQueryable(new List<string>() { "Prices", "Category" });
            if (categoryId != null)
            {
                query = query.Where(c => c.CategoryId.Equals(categoryId));
            }

            var items = await query.ToListAsync();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "products.xlsx");
            string fileName = $"{Guid.NewGuid()}.xlsx";
            string newFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", $"danh-sach-hang-hoa-{fileName}");
            var file = new FileInfo(newFile);
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
            {
                var sheet = excelPackage.Workbook.Worksheets.First();
                var startRow = 7;
                for (int index = 0; index < items.Count; index++)
                {
                    var product = items[index];
                    var prices = product.Prices.OrderByDescending(c => c.IsDefault)
                        .ThenBy(c => c.TransferQuantity).ToList();

                    sheet.Cells[$"A{startRow}"].Value = index + 1;

                    for (int sub = 0; sub < prices.Count; sub++)
                    {
                        if (prices[sub].IsDefault)
                        {
                            sheet.Cells[$"B{startRow}"].Value = product.Name;
                            sheet.Cells[$"D{startRow}"].Value = product.Category.Name;

                        }
                        sheet.Cells[$"C{startRow}"].Value = ContantsUnit.GetUnit(prices[sub].UnitId).Name;
                        sheet.Cells[$"E{startRow}"].Value = prices[sub].ImportPrice;
                        sheet.Cells[$"F{startRow}"].Value = prices[sub].SellPrice;
                        sheet.Cells[$"G{startRow}"].Value = prices[sub].WholeSalePrice;

                        startRow++;
                    }

                }

                excelPackage.SaveAs(file);
                return fileName;
            }

        }

        public async Task ImportProducts(IFormFile file)
        {
            var data = new List<ProductImportExcel>();
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {

                    file.CopyTo(ms);
                    var content = ms.ToArray();
                    var stream = new MemoryStream(content);

                    using (var excelPackage = new ExcelPackage(stream))
                    {
                        var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                        int colCount = workSheet.Dimension.End.Column;
                        int rowCount = workSheet.Dimension.End.Row;
                        for (int i = 5; i <= rowCount; i++)
                        {
                            string cat = workSheet.Cells[$"B{i}"].Value?.ToString() ?? string.Empty;
                            string barCode = workSheet.Cells[$"C{i}"].Value?.ToString() ?? string.Empty;
                            string name = workSheet.Cells[$"D{i}"].Value?.ToString() ?? string.Empty;
                            string SellPrice = workSheet.Cells[$"E{i}"].Value?.ToString() ?? "0";
                            string importPrice = workSheet.Cells[$"F{i}"].Value?.ToString() ?? "0";
                            string unit = workSheet.Cells[$"G{i}"].Value?.ToString() ?? "Cái";
                            string wholeSale = workSheet.Cells[$"M{i}"].Value?.ToString() ?? "0";
                            string transferQuantity = workSheet.Cells[$"L{i}"].Value?.ToString() ?? "0";
                            data.Add(new ProductImportExcel()
                            {
                                BarCode = barCode,
                                Category = cat,
                                ImportPrice = importPrice,
                                Name = name,
                                SellPrice = SellPrice,
                                TransferQuantity = transferQuantity,
                                Unit = unit,
                                WholeSalePrice = wholeSale
                            });
                            
                        }
                        data.RemoveAll(c => string.IsNullOrEmpty(c.BarCode) && string.IsNullOrEmpty(c.Name));
                        var barcodes = (await _repository.FindAllAsync(null, null)).Select(c => c.BarCode.Trim());
                        data.RemoveAll(c => barcodes.Contains(c.BarCode));
                        var excute = await ConvertToProduct(data);
                        await _repository.AddRangeAsync(excute.products);
                        await _priceRepository.AddRangeAsync(excute.prices);
                    }
                }

            }
            return;
        }

        public async Task<ProductUpdatePrice> GetProductUpdate(Guid id)
        {
            var quantity = await GetRemainQuantity(id);
            var data = new ProductUpdatePrice(await GetById(id),quantity.Quantity);

            return data;
        }

        private async Task<(List<Product> products, List<Price> prices)> ConvertToProduct(List<ProductImportExcel> data)
        {
            var products = new List<Product>();
            var prices = new List<Price>();

            var categories = (await _catRepository.FindAllAsync(null, null)).ToList();
            var units = ContantsUnit.Units();
            var id = Guid.Empty;
            foreach (var item in data)
            {
                // Nếu barcode null thì đó là 1 loại của sản phẩm trước đó
                if (!string.IsNullOrEmpty(item.BarCode))
                {
                    var prod = item.ToProduct(categories, units);
                    id = prod.Id;
                    var price = item.ToPrice(units, id, true);
                    if (price != null)
                    {
                        prices.Add(price);
                    }
                    if (prod != null)
                    {
                        
                        products.Add(prod);
                    }
                    if(price is null || prod is null)
                    {

                    }
                }
                else
                {
                    var price = item.ToPrice(units, id, false);
                    if (price != null)
                    {
                        prices.Add(price);
                    }
                    if(price is null)
                    {

                    }
                }
            }

            await Task.Yield();
            return (products, prices);
        }

        private async Task<Product> GetById(Guid id) => await _repository.FindAsync(id, new List<string>() { "Category", "Prices" });

        private async Task<InventoryItem> GetRemainQuantity(Guid id)
        {
            var inventory = await _inventoryRepository.FindAsync(c => c.ProductId.Equals(id));

            return inventory;
        }
        public async Task UpdateProductPrice(ProductUpdatePrice price)
        {
            await _priceRepository.DeleteRangeAsync(c => c.ProductId == price.Id);
            var entities = price.Prices.Select(c => c.ToEntity(price.Id)).OrderBy(c => c.TransferQuantity);
            await _priceRepository.AddRangeAsync(entities);
            var inventory = await _inventoryRepository.FindAsync(c => c.ProductId == price.Id);
            inventory.Quantity = price.TotalQuantity.Value;
            await _inventoryRepository.UpdateAsync(inventory);
        }

        public async Task<IEnumerable<PrintPrice>> GetPrintPrices(IEnumerable<Guid> ids)
        {
            var prices = await _priceRepository.FindAllAsync(c => ids.Contains(c.ProductId),new string[] {"Product"});
            return prices.Where(c => c.IsDefault).Select(c => new PrintPrice()
            {
                Name = c.Product.Name.ToUpper(),
                Price = c.SellPrice.ToCurrencyFormat(),
                Title = "KHUYẾN MẠI",
                Unit = ContantsUnit.GetUnit(c.UnitId)?.Name ?? ""
            });
        }


    }
}
