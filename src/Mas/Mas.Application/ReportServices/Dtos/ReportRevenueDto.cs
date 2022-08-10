using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ReportServices.Dtos
{
    public class ReportRevenue
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double SumImport { get; set; }

        public double SumSell { get; set; }

        public double SumDiscount { get; set; }

        public double SumProfit { get; set; }

        public IEnumerable<ReportRevenueDto> Items { get; set; }

    }
    public class ReportRevenueDto
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
