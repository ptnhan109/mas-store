using Mas.Common;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class ProductImportExcel
    {
        public string Category { get; set; }

        public string BarCode { get; set; }

        public string Name { get; set; }

        public string SellPrice { get; set; }

        public string ImportPrice { get; set; }

        public string Unit { get; set; }

        public string TransferQuantity { get; set; }

        public string WholeSalePrice { get; set; }

        public Product ToProduct(List<Category> categories, List<Unit> units)
        {
            var cate = categories.FirstOrDefault(c => c.Name.ToRemoveUnicode().Trim() == Category.ToRemoveUnicode().Trim());
            var unit = units.FirstOrDefault(c => c.Name.ToRemoveUnicode().Trim().Equals(Unit.ToRemoveUnicode().Trim()));
            var currentTime = DateTime.Now;
            if (cate != null && unit != null)
            {
                var prod = new Product()
                {
                    BarCode = BarCode,
                    CategoryId = cate.Id,
                    InventoryLimit = 0,
                    Name = Name,
                    Id = Guid.NewGuid(),
                    SearchParams = $"{BarCode} {Name}".ToRemoveUnicode(),
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime
                };
                return prod;
            }

            return default;
        }
        public Price ToPrice(List<Unit> units, Guid id,bool isDefault)
        {
            var unit = units.FirstOrDefault(c => c.Name.Trim().Equals(Unit.Trim()));
            if(unit != null)
            {
                var currentDate = DateTime.Now;
                var price = new Price()
                {
                    BarCode = BarCode,
                    CreatedAt = currentDate,
                    Id = Guid.NewGuid(),
                    ImportPrice = double.Parse(ImportPrice),
                    ProductId = id,
                    SellPrice = double.Parse(SellPrice),
                    UnitId = unit.Id,
                    UpdatedAt = DateTime.Now,
                    WholeSalePrice = double.Parse(WholeSalePrice),
                    IsDefault = isDefault,
                    SearchParams = String.Empty,
                    TransferQuantity = isDefault ? 1 : int.Parse(TransferQuantity),
                };

                return price;
            }

            return default;
        }
    }
}
