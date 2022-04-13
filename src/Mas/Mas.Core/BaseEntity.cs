using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Core
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public string SearchParams { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
