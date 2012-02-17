using System.Collections.Generic;

namespace Hygia.Widgets.Features
{
    public class PieChartItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Colour { get; set; }
    }

    public class PieChart
    {
        public PieChart()
        {
            Item = new List<PieChartItem>();
        }

        public IList<PieChartItem> Item { get; set; }
    }
}