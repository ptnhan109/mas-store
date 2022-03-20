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
            Inventory = product.Inventory;
            CategoryId = product.CategoryId;
            UnitId = defaultPrice.UnitId;
            DefaultImportPrice = defaultPrice.ImportPrice;
            DefaultSellPrice = defaultPrice.SellPrice;
            DefaultPriceId = defaultPrice.Id;

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
}
