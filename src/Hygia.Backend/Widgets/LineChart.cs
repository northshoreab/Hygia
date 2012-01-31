using System.Collections.Generic;

namespace Hygia.Backend.Widgets
{
    public class LineChartSettings
    {
        public LineChartSettings()
        {
            AxisX = new List<string>();
            AxisY = new List<string>();
        }

        public IList<string> AxisX { get; set; }
        public IList<string> AxisY { get; set; }
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