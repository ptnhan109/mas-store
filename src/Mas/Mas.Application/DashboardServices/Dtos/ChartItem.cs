using System;
using System.Collections.Generic;
using System.Text;

namespace Mas.Application.DashboardServices.Dtos
{
    public class ChartItem
    {
        public string Label { get; set; }

        public List<double> Data { get; set; }

        public List<string> BackgroundColor { get; set; }

        public List<string> BorderColor { get; set; }

        public int BorderWidth { get; set; } = 1;
    }

    public class Chart
    {
        public List<string> Labels { get; set; }

        public List<ChartItem> Datasets { get; set; }
    }
}
