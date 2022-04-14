using Mas.Common;
using Mas.Core.Contants;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class AddDestruction
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public List<AddDestructionItem> Items { get; set; }

        public Destruction ToEntity()
        {
            var entity = new Destruction()
            {
                Code = Code,
                Description = Description,
                CreatedBy = CreatedBy,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                Amount = 0
            };

            if (string.IsNullOrEmpty(entity.Code))
            {
                entity.Code = "HH".GenerateCode(10);
            }

            entity.SearchParams = $"{entity.Code.ToRemoveUnicode()} - {entity.Description.ToRemoveUnicode()}";
            Items.ForEach(item =>
            {
                entity.Amount += item.Quantity * item.CurrentPrice;
            });

            return entity;
        }
    }

    public class AddDestructionItem
    {
        public string Name { get; set; }

        public string UnitName { get; set; }

        public string BarCode { get; set; }

        public double CurrentPrice { get; set; }

        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public DestructionDetail ToEntity(Guid destructionId) => new DestructionDetail()
        {
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            Amount = Quantity * CurrentPrice,
            Quantity = Quantity,
            DestructionId = destructionId,
            CurrentImport = CurrentPrice,
            Name = Name,
            SearchParams = string.Empty,
            ProductId = ProductId,
            UnitId = ContantsUnit.GetId(UnitName)
        };
    }
}
