using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core.Entities
{
    public class ManufactureGroup : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Manufacture> Manufactures { get; set; }
    }
}
