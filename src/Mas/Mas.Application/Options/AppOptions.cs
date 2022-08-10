using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.Options
{
    public class AppOptions
    {
        public string Secret { get; set; }

        public Guid DefaultCustomerId { get; set; }
    }
}
