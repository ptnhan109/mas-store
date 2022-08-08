using Mas.Common;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class ProductDetail : AddProductModel
    {
        public Guid Id { get; set; }

        public Guid DefaultPriceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ProductDetail()
        {

        }

        public ProductDetail(Product product)
        {
            var defaultPrice = product.Prices.FirstOrDefault(c => c.IsDefault);
            Id = product.Id;
            BarCode = product.BarCode;
            Name = product.Name;
            CurrentQuantity = product.InventoryLimit;
            CategoryId = product.CategoryId;
            UnitId = defaultPrice.UnitId;
            DefaultImportPrice = defaultPrice.ImportPrice;
            DefaultSellPrice = defaultPrice.SellPrice;
            DefaultPriceId = defaultPrice.Id;
            WholeSellPrice = defaultPrice.WholeSalePrice;

            CreatedAt = product.CreatedAt;
            UpdatedAt = product.UpdatedAt;

            var prices = product.Prices.Where(c => !c.IsDefault);
            if (prices.Any())
            {
                Prices = prices.Select(c => new AddPriceModel(c)).ToList();
            }
        }

        public override Product ToProduct()
        {
            var prod = base.ToProduct();
            prod.Id = Id;

            return prod;
        }
    }

    public class UpdateProductRequest : AddProductModel
    {
        public Guid Id { get; set; }

        public override Product ToProduct()
        {
            var product = new Product()
            {
                Id = Id,
                BarCode = BarCode,
                CategoryId = CategoryId,
                Name = Name,
                InventoryLimit = InventoryLimit,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SearchParams = Name.ToRemoveUnicode()
            };

            var defaultPrice = new Price()
            {
                BarCode = BarCode,
                UnitId = UnitId,
                TransferQuantity = 1,
                Id = Guid.NewGuid(),
                ImportPrice = DefaultImportPrice,
                SellPrice = DefaultSellPrice,
                ProductId = Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDefault = true,
                WholeSalePrice = WholeSellPrice
            };

            var prices = new List<Price>() { defaultPrice };

            if (Prices.Any())
            {
                prices.AddRange(Prices.Select(c => c.ToPrice(Id)));
            }

            product.Prices = prices;

            return product;
        }
    }
}
