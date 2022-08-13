using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class ProductUpdatePrice
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }

        public IEnumerable<UpdatePriceItem> Prices { get; set; }

        public ProductUpdatePrice()
        {

        }

        public ProductUpdatePrice(Product product)
        {
            Id = product.Id;
            ProductName = product.Name;
            Category = product.Category.Name;
            Prices = product.Prices.Select(c => new UpdatePriceItem()
            {
                BarCode = c.BarCode,
                ImportPrice = c.ImportPrice,
                IsDefault = c.IsDefault,
                SellPrice = c.SellPrice,
                TransferQuantity = c.TransferQuantity,
                UnitId = c.UnitId,
                WholeSalePrice = c.WholeSalePrice
            });
        }
    }

    public class UpdatePriceItem
    {
        public string BarCode { get; set; }

        public int UnitId { get; set; }

        public int TransferQuantity { get; set; }

        public double ImportPrice { get; set; }

        public double SellPrice { get; set; }

        public double WholeSalePrice { get; set; }

        public bool IsDefault { get; set; }

        public Price ToEntity(Guid id) => new Price()
        {
            Id = Guid.NewGuid(),
            BarCode = BarCode,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ImportPrice = ImportPrice,
            IsDefault = IsDefault,
            SellPrice = SellPrice,
            ProductId = id,
            TransferQuantity = TransferQuantity,
            UnitId = UnitId,
            WholeSalePrice = WholeSalePrice
        };
    }


}
