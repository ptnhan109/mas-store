using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Price : BaseEntity
    {
        public int ParentUnitId { get; set; }

        public int ChildUnitId { get; set; }

        public int TransferQuantity { get; set; }

        public double ParentImportPrice { get; set; }
        
        public double ChildImportPrice { get; set; }

        public double ParentSellPrice { get; set; }

        public double ChildSellPrice { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
