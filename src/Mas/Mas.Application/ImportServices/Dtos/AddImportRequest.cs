using Mas.Common;
using Mas.Core.Contants;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ImportServices.Dtos
{
    public class AddImportRequest
    {
        public string CreatedBy { get; set; }

        public string Code { get; set; }

        public string Note { get; set; }

        public double Discount { get; set; }

        public Guid ManufactureId { get; set; }

        public List<ImportItem> Items { get; set; }

        public Import ToEntity()
        {
            double amount = 0;
            Items.ForEach(item =>
            {
                amount += item.Quantity * (item.CurrentPrice - item.Discount);
            });
            if (string.IsNullOrEmpty(Code))
            {
                Code = $"NH{DateTime.Now.ToString("yyyyMMdd")}".GenerateCode(5);
            }
            return new Import()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SearchParams = $"{Code.ToRemoveUnicode()} {Note.ToRemoveUnicode()}",
                Amount = amount - Discount,
                Discount = Discount,
                Id = Guid.NewGuid(),
                CreatedBy = CreatedBy,
                ManufactureId = ManufactureId,
                Note = Note,
                Code = Code
            };
        }
    }

    public class ImportItem
    {
        public string Name { get; set; }

        public string UnitName { get; set; }

        public string BarCode { get; set; }

        public double CurrentPrice { get; set; }

        public double Discount { get; set; }

        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public ImportDetail ToEntity(Guid id)
        {
            var entity = new ImportDetail()
            {
                Id = Guid.NewGuid(),
                BarCode = BarCode,
                CreatedAt = DateTime.Now,
                CurrentPrice = CurrentPrice,
                Discount = Discount,
                Amount = Math.Round((CurrentPrice - Discount) * Quantity),
                ImportId = id,
                Name = Name,
                ProductId = ProductId,
                Quantity = Quantity,
                SearchParams = string.Empty,
                UnitId = ContantsUnit.GetId(UnitName),
                UpdatedAt = DateTime.Now
            };

            return entity;
        }
    }
}
