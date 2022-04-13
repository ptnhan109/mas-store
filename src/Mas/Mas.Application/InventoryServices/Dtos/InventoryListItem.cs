using Mas.Application.ProductServices.Dtos;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class InventoryListItem
    {
        public Guid Id { get; set; }

        public ProductInventoryItem Product { get; set; }

        public int Quantity { get; set; }

        public string LastDayImport { get; set; }

        public static InventoryListItem FromEntity(InventoryItem entity)
        {
            return new InventoryListItem()
            {
                Product = new ProductInventoryItem(entity.Product),
                Quantity = entity.Quantity,
                LastDayImport = entity.UpdatedAt.ToString("dd-MM-yyyy"),
                Id = entity.Id
            };
        }
    }
}


