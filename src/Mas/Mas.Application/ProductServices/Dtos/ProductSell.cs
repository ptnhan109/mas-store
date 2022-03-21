using Mas.Core.Contants;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class ProductSell
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string BarCode { get; set; }
        
        public List<PriceItem> Prices { get; set; }

        public ProductSell(Product product, bool isWholeSale)
        {
            Id = product.Id;
            Name = product.Name;
            BarCode = product.BarCode;
            Prices = product.Prices.Select(c => new PriceItem(c, isWholeSale)).ToList();
        }
    }

    public class PriceItem {
        public string BarCode { get; set; }

        public Unit Unit { get; set; }

        public double SellPrice { get; set; }

        public double Discount { get; set; }

        public int Quantity { get; set; }

        public double TotalMoney { get; set; }

        public bool IsDefault { get; set; }

        public PriceItem(Price price, bool isWholeSale)
        {
            BarCode = price.BarCode;
            Unit = ContantsUnit.GetUnit(price.UnitId);
            if (isWholeSale)
            {
                SellPrice = price.WholeSalePrice;
            }
            else
            {
                SellPrice = price.SellPrice;
            }
            Discount = 0;
            Quantity = 1;
            TotalMoney = SellPrice * Quantity;
            IsDefault = price.IsDefault;
        }
    }
}
