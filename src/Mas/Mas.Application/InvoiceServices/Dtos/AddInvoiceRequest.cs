using Mas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.InvoiceServices.Dtos
{
    public class AddInvoiceRequest
    {
        public Guid? CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Note { get; set; }

        public double Discount { get; set; }

        public List<AddInvoiceDetail> InvoiceDetails { get; set; }
    }

    public class AddInvoiceDetail
    {
        public string Name { get; set; }

        public string UnitName { get; set; }

        public string BarCode { get; set; }

        public double CurrentPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }
    }
}
