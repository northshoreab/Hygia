using System.Collections.Generic;

namespace Hygia.Widgets.Widgets
{
    public class LineChartSettings
    {
        public LineChartSettings()
        {
            Axisx = new List<string>();
            Axisy = new List<string>();
        }

        public IList<string> Axisx { get; set; }
        public IList<string> Axisy { get; set; }
        public string Colour { get; set; }
    }

    public class LineChart
    {
        public LineChart()
        {
            Item = new List<string>();
        }

        public IList<string> Item { get; set; }
        public LineChartSettings Settings { get; set; }
    }
}