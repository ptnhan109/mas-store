using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ProductServices.Dtos
{
    public class PrintBarcode
    {
        public int Quantity { get; set; }

        public string Name { get; set; }
    }

    public class PrintPrice
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Unit { get; set; }
    }

    public class PrintPrices
    {
        public IEnumerable<Guid> ids { get; set; }
    }
}
