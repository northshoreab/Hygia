using System.Collections.Generic;

namespace Hygia.Backend.Widgets
{
    public class LineChartSettings
    {
        public LineChartSettings()
        {
            axisx = new List<string>();
            axisy = new List<string>();
        }

        public IList<string> axisx { get; set; }
        public IList<string> axisy { get; set; }
        public string colour { get; set; }
    }

    public class LineChart
    {
        public LineChart()
        {
            item = new List<string>();
        }

        public IList<string> item { get; set; }
        public LineChartSettings settings { get; set; }
    }
}