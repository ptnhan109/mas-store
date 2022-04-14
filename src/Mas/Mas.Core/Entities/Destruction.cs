using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mas.Core.Entities
{
    public class Destruction : BaseEntity
    {
        [StringLength(30)]
        public string Code { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public double Amount { get; set; }

        public virtual ICollection<DestructionDetail> DestructionDetails { get; set; }
    }
}
