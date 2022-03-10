using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class AddProductModel
    {
        public string BarCode { get; set; }

        public string Name { get; set; }

        public double DefaultSellPrice { get; set; }

        public double DefaultImportPrice { get; set; }

        public int Inventory { get; set; }

        public Guid CategoryId { get; set; }

        public int? CloseToDate { get; set; }

        public int UnitId { get; set; }


        public int? ParentUnitId { get; set; }

        public int? TransferQuantity { get; set; }

        public double? ParentImportPrice { get; set; }

        public double? ParentSellPrice { get; set; }

        public string TransferBarCode { get; set; }

        public virtual Product ToProduct()
        {
            Guid id = Guid.NewGuid();
            var product = new Product()
            {
                Id = id,
                BarCode = BarCode,
                CategoryId = CategoryId,
                Name = Name,
                Inventory = Inventory,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var defaultPrice = new Price()
            {
                BarCode = BarCode,
                UnitId = UnitId,
                TransferQuantity = 1,
                Id = Guid.NewGuid(),
                ImportPrice = DefaultImportPrice,
                SellPrice = DefaultSellPrice,
                ProductId = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDefault = true
            };

            var prices = new List<Price>() { defaultPrice };

            if(ParentUnitId != null)
            {
                var price = new Price()
                {
                    BarCode = TransferBarCode,
                    Id = Guid.NewGuid(),
                    ImportPrice = ParentImportPrice.Value,
                    SellPrice = ParentSellPrice.Value,
                    ProductId = id,
                    IsDefault = false,
                    TransferQuantity = TransferQuantity.Value,
                    UnitId = ParentUnitId.Value,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                prices.Add(price);
            }

            product.Prices = prices;

            return product;
        }
        
    }
}
