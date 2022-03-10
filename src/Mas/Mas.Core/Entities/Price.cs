using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Price : BaseEntity
    {
        public string BarCode { get; set; }

        public int UnitId { get; set; }

        public int TransferQuantity { get; set; }

        public double ImportPrice { get; set; }

        public double SellPrice { get; set; }

        public Guid ProductId { get; set; }

        public bool IsDefault { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
