using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Import : BaseEntity
    {
        public Guid? ManufactureId { get; set; }
    
        [ForeignKey(nameof(ManufactureId))]
        public virtual Manufacture Manufacture { get; set; }

        public double Amount { get; set; }

        public double Discount { get; set; }

        public string CreatedBy { get; set; }

        public virtual ICollection<ImportDetail> ImportDetails { get; set; }

        public string Note { get; set; }

        public string Code { get; set; }
    }
}
