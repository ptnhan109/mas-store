using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ReportServices.Dtos
{
    public class ReportRevenueFilter
    {
        public Guid? EmployeeId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
