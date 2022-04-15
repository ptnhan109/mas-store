using Mas.Common;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class DestructionItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Amount { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public static DestructionItem FromEntity(Destruction entity)
        {
            return new DestructionItem()
            {
                Id = entity.Id,
                Code = entity.Code,
                Amount = entity.Amount.ToCurrencyFormat(),
                CreatedDate = entity.CreatedAt.ToString("dd-MM-yyyy"),
                CreatedBy = entity.CreatedBy.Trim()
            };
        }
    }
}
