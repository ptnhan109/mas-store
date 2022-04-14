using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Product : BaseEntity
    {
        [StringLength(55)]
        public string BarCode { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Price> Prices { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public int InventoryLimit { get; set; }
    }
}
