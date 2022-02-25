using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Product : BaseEntity
    {
        public string BarCode { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid PriceId { get; set; }

        [ForeignKey(nameof(PriceId))]
        public virtual ICollection<Price> Price { get; set; }

        public double DefaultSellPrice { get; set; }

        public double DefaultImportPrice { get; set; }
    }
}
