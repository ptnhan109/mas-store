using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        public string Name { get; set; }

        public int UnitId { get; set; }

        public string BarCode { get; set; }

        public double CurrentPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public double Amount { get; set; }

    }
}
