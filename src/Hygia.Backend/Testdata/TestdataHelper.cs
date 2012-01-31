using System.Collections.Generic;
using System.Drawing;
using Hygia.Backend.Widgets;

namespace Hygia.Backend.Testdata
{
    public static class TestdataHelper
    {
        static readonly char[] HexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        public static string ColorToHexString(Color color)
        {
            var bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            var chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = HexDigits[b >> 4];
                chars[i * 2 + 1] = HexDigits[b & 0xF];
            }

            return new string(chars);
        }

        public static Funnel GetFunnel()
        {
            var funnel = new Funnel {Percentage = FunnelPercentage.Show, Type = FunnelType.Reverse};

            funnel.Item.Add(new FunnelItem
                                {
                                    Label = "Order placed",
                                    Value = "500"
                                });

            funnel.Item.Add(new FunnelItem
                                {
                                    Label = "Order paid",
                                    Value = "300"
                                });

            funnel.Item.Add(new FunnelItem
                                {
                                    Label = "Order delivered",
                                    Value = "200"
                                });

            return funnel;
        }

        public static PieChart GetPieChart()
        {
            var pieChart = new PieChart();

            pieChart.Item.Add(new PieChartItem
                                  {
                                      Colour = ColorToHexString(Color.Blue),
                                      Label = "Order placed",
                                      Value = "500"
                                  });

            pieChart.Item.Add(new PieChartItem
                                  {
                                      Colour = ColorToHexString(Color.Green),
                                      Label = "Order paid",
                                      Value = "300"
                                  });
            pieChart.Item.Add(new PieChartItem
                                  {
                                      Colour = ColorToHexString(Color.Yellow),
                                      Label = "Order delivered",
                                      Value = "200"
                                  });

            return pieChart;
        }

        public static LineChart GetLineChart()
        {
            var lineChart = new LineChart();

            lineChart.Item.Add("500");
            lineChart.Item.Add("600");
            lineChart.Item.Add("531");
            lineChart.Settings = new LineChartSettings
                                     {
                                         AxisX = new List<string>{ "Jun", "Jul", "Aug" },
                                         AxisY = new List<string>{ "Min", "Max" },
                                         Colour = ColorToHexString(Color.Yellow)
                                     };

            return lineChart;
        }
    }
}