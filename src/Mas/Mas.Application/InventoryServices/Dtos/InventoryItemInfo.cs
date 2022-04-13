using Mas.Common;
using Mas.Core.Contants;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class InventoryItemInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public List<InventoryPriceItem> Prices { get; set; }

        public InventoryItemInfo(InventoryItem entity)
        {
            Id = entity.Id;
            Name = entity.Product.Name;
            Category = entity.Product.Category.Name;
            var inventoryPrices = new List<InventoryPriceItem>();
            int quantity = entity.Quantity;
            var prices = entity.Product.Prices.OrderByDescending(c => c.TransferQuantity).ToList();
            for (int index = 0; index < prices.Count; index++)
            {
                var indexQuantity = (quantity - (quantity % prices[index].TransferQuantity)) / prices[index].TransferQuantity;
                quantity = quantity % prices[index].TransferQuantity;
                var price = new InventoryPriceItem()
                {
                    ImportPrice = prices[index].ImportPrice.ToCurrencyFormat(),
                    SellPrice = prices[index].SellPrice.ToCurrencyFormat(),
                    Unit = ContantsUnit.GetUnit(prices[index].UnitId).Name,
                    WholeSellPrice = prices[index].WholeSalePrice.ToCurrencyFormat(),
                    Quantity = indexQuantity
                };

                inventoryPrices.Add(price);
            }
            Prices = inventoryPrices;
        }
    }

    public class InventoryPriceItem
    {
        public string Unit { get; set; }

        public string ImportPrice { get; set; }

        public string SellPrice { get; set; }

        public string WholeSellPrice { get; set; }

        public int Quantity { get; set; }
    }
}
