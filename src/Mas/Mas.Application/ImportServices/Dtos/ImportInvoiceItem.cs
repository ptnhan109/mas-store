using Mas.Common;
using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ImportServices.Dtos
{
    public class ImportInvoiceItem
    {
        public Guid Id { get; set; }

        public string Manufacture { get; set; }

        public string CreatedBy { get; set; }

        public string Amount { get; set; }

        public string Code { get; set; }

        public string CreatedDate { get; set; }

        public static ImportInvoiceItem FromEntity(Import import)
        {
            return new ImportInvoiceItem()
            {
                Id = import.Id,
                Manufacture = import.Manufacture.Name,
                CreatedBy = import.CreatedBy,
                Amount = import.Amount.ToCurrencyFormat(),
                Code = import.Code,
                CreatedDate = import.CreatedAt.ToString("dd-MM-yyyy")
            };
        }
    }
}
