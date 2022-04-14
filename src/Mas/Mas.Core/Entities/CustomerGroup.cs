﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mas.Core.Entities
{
    public class CustomerGroup : BaseEntity
    {
        public string Name { get; set; }

        [StringLength(30)]
        public string Code { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
