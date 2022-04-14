using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mas.Core.Entities
{
    public class Category : BaseEntity
    {

        [StringLength(30)]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public bool IsDeleted { get; set; }

        public string Location { get; set; }
    }
}
