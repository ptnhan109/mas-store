using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
