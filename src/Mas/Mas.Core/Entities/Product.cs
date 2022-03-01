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

        public virtual ICollection<Price> Price { get; set; }

        public double DefaultSellPrice { get; set; }

        public double DefaultImportPrice { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public int Inventory { get; set; }

        public int? CloseToDate { get; set; }

        public int UnitId { get; set; }
    }
}
