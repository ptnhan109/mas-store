using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mas.Core.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Province { get; set; }

        public string Mail { get; set; }

        public Guid? GroupId { get; set; }

        public int Code { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual CustomerGroup CustomerGroup { get; set; }
    }
}
