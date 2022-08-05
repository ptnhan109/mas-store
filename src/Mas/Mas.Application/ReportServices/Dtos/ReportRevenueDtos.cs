using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ReportServices.Dtos
{
    public class ReportRevenueDtos
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public double ImportPrice { get; set; }

        public double SellPrice { get; set; }

        public double Profit { get; set; }

        public string User { get; set; }

        public string Category { get; set; }

        public double Discount { get; set; }

        public int Unit { get; set; }

        public int TransferQuantity { get; set; }
    }
}
