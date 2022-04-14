using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class DestructionDetail : BaseEntity
    {
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public int UnitId { get; set; }

        public double CurrentImport { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }

        public Guid DestructionId { get; set; }
        
        [ForeignKey(nameof(DestructionId))]
        public virtual Destruction Destruction { get; set; }

        public string Name { get; set; }
    }
}
