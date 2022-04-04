using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class AddInventoryItem
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public AddInventoryItem(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public InventoryItem ToEntity()
        {
            return new InventoryItem()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ProductId = ProductId,
                Quantity = Quantity,
                Id = Guid.NewGuid()
            };
        }
    }
}
