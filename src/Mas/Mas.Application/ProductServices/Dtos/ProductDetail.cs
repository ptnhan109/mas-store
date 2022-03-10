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

        public Guid? TransferId { get; set; }

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

            var price = product.Prices.FirstOrDefault(c => !c.IsDefault);
            if (price != null)
            {
                ParentUnitId = price.UnitId;
                TransferQuantity = price.TransferQuantity;
                TransferId = price.Id;
                ParentImportPrice = price.ImportPrice;
                ParentSellPrice = price.SellPrice;
                TransferBarCode = price.BarCode;

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
