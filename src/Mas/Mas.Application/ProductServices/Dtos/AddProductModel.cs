using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class AddProductModel
    {
        public string BarCode { get; set; }

        public string Name { get; set; }

        public double DefaultSellPrice { get; set; }

        public double DefaultImportPrice { get; set; }

        public double WholeSellPrice { get; set; }

        public int CurrentQuantity { get; set; }

        public int InventoryLimit { get; set; }

        public Guid CategoryId { get; set; }

        public int? CloseToDate { get; set; }

        public int UnitId { get; set; }

        public List<AddPriceModel> Prices { get; set; }

        public virtual Product ToProduct()
        {
            Guid id = Guid.NewGuid();
            var product = new Product()
            {
                Id = id,
                BarCode = BarCode,
                CategoryId = CategoryId,
                Name = Name,
                InventoryLimit = InventoryLimit,
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
                IsDefault = true,
                WholeSalePrice = WholeSellPrice
            };

            var prices = new List<Price>() { defaultPrice };

            if (Prices.Any())
            {
                prices.AddRange(Prices.Select(c => c.ToPrice(id)));
            }

            product.Prices = prices;

            return product;
        }

    }

    public class AddPriceModel
    {
        public int ParentUnitId { get; set; }

        public int TransferQuantity { get; set; }

        public double ParentImportPrice { get; set; }

        public double ParentSellPrice { get; set; }

        public string TransferBarCode { get; set; }

        public double WholeSalePrice { get; set; }

        public AddPriceModel()
        {

        }

        public AddPriceModel(Price entity)
        {
            ParentImportPrice = entity.ImportPrice;
            ParentSellPrice = entity.SellPrice;
            TransferQuantity = entity.TransferQuantity;
            ParentUnitId = entity.UnitId;
            TransferBarCode = entity.BarCode;
            WholeSalePrice = entity.WholeSalePrice;
        }

        public Price ToPrice(Guid id)
        {
            return new Price()
            {
                BarCode = TransferBarCode,
                Id = Guid.NewGuid(),
                ImportPrice = ParentImportPrice,
                SellPrice = ParentSellPrice,
                ProductId = id,
                IsDefault = false,
                TransferQuantity = TransferQuantity,
                UnitId = ParentUnitId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                WholeSalePrice = WholeSalePrice
            };
        }
    }
}
