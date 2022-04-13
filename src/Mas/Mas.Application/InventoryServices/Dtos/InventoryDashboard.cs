using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mas.Application.InventoryServices.Dtos
{
    public class InventoryDashboard
    {
        public int TotalProductQuantity { get; set; }

        public double TotalMoney { get; set; }

        public int BelowQuota { get; set; }
    }
}
