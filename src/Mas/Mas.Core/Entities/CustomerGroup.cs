using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.Entities
{
    public class CustomerGroup : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
