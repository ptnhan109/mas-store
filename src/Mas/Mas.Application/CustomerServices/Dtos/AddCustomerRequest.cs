using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.CustomerServices.Dtos
{
    public class AddCustomerRequest
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
