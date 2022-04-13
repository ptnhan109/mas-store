using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Manufacture : BaseEntity
    {
        public string Name { get; set; }

        public string Mail { get; set; }

        public string Phone { get; set; }

        public Guid? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual ManufactureGroup Group { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string Note { get; set; }

        public string Code { get; set; }

        public string TaxCode { get; set; }
    }
}
