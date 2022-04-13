using Mas.Core.Contants;
using Mas.Core.Entities;
using Mas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.InvoiceServices.Dtos
{
    public class AddInvoiceRequest
    {
        public Guid? CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Note { get; set; }

        public double Discount { get; set; }

        public List<AddInvoiceDetail> InvoiceDetails { get; set; }

        public Invoice ToEntity()
        {
            double amount = 0;
            InvoiceDetails.ForEach(c =>
            {
                amount += c.Quantity * (c.CurrentPrice - c.Discount);
            });
            return new Invoice()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Type = EnumInvoice.Sell,
                CustomerId = CustomerId,
                Discount = Discount,
                Amount = amount - Discount
            };
        }
    }

    public class AddInvoiceDetail
    {
        public string Name { get; set; }

        public string UnitName { get; set; }

        public string BarCode { get; set; }

        public double CurrentPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public Guid ProductId { get; set; }

        public InvoiceDetail ToEntity(Guid id) => new InvoiceDetail()
        {
            Amount = Math.Round((CurrentPrice - Discount) * Quantity),
            Name = string.Empty,
            UnitId = ContantsUnit.GetId(UnitName),
            BarCode = BarCode,
            CurrentPrice = CurrentPrice,
            Quantity = Quantity,
            Discount = Discount,
            CurrentImport = 0,
            Profit = 0,
            Id = Guid.NewGuid(),
            InvoiceId = id,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ProductId = ProductId
        };
    }
}
