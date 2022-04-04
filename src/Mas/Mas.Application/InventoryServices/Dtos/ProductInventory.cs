using Mas.Application.ProductServices.Dtos;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class ProductInventory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int Quantity { get; set; }

        public string DateImport { get; set; }

        public IEnumerable<PriceItem> PriceItems { get; set; }

        public ProductInventory(InventoryItem entity)
        {
            Id = entity.Id;
            Name = entity.Product.Name;
            Category = entity.Product.Category.Name;
            Quantity = entity.Quantity;
            DateImport = string.Empty;
            PriceItems = entity.Product.Prices.Select(c => new PriceItem(c, false));
        }
    }
}
