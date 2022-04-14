using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Mas.Core.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        public string Name { get; set; }

        public int UnitId { get; set; }

        [StringLength(55)]
        public string BarCode { get; set; }

        public double CurrentPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public double Amount { get; set; }

        public double CurrentImport { get; set; }

        public double Profit { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
