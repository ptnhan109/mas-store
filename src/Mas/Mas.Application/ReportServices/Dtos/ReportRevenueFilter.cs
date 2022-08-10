using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.ReportServices.Dtos
{
    public class ReportRevenueFilter
    {
        public Guid? UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? CategoryId { get; set; }

        public string Keyword { get; set; }

        public Guid? CustomerId { get; set; }
    }
}
