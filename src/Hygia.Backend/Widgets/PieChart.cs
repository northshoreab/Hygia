using System.Collections.Generic;

namespace Hygia.Backend.Widgets
{
    public class PieChartItem
    {
        public string label { get; set; }
        public string value { get; set; }
        public string colour { get; set; }
    }

    public class PieChart
    {
        public PieChart()
        {
            item = new List<PieChartItem>();
        }

        public IList<PieChartItem> item { get; set; }
    }
}