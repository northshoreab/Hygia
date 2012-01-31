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
            var funnel = new Funnel {percentage = FunnelPercentage.Show, type = FunnelType.Reverse};

            funnel.item.Add(new FunnelItem
                                {
                                    label = "Order placed",
                                    value = "500"
                                });

            funnel.item.Add(new FunnelItem
                                {
                                    label = "Order paid",
                                    value = "300"
                                });

            funnel.item.Add(new FunnelItem
                                {
                                    label = "Order delivered",
                                    value = "200"
                                });

            return funnel;
        }

        public static PieChart GetPieChart()
        {
            var pieChart = new PieChart();

            pieChart.item.Add(new PieChartItem
                                  {
                                      colour = ColorToHexString(Color.Blue),
                                      label = "Order placed",
                                      value = "500"
                                  });

            pieChart.item.Add(new PieChartItem
                                  {
                                      colour = ColorToHexString(Color.Green),
                                      label = "Order paid",
                                      value = "300"
                                  });
            pieChart.item.Add(new PieChartItem
                                  {
                                      colour = ColorToHexString(Color.Yellow),
                                      label = "Order delivered",
                                      value = "200"
                                  });

            return pieChart;
        }

        public static LineChart GetLineChart()
        {
            var lineChart = new LineChart();

            lineChart.item.Add("500");
            lineChart.item.Add("600");
            lineChart.item.Add("531");
            lineChart.settings = new LineChartSettings
                                     {
                                         axisx = new List<string>{ "Jun", "Jul", "Aug" },
                                         axisy = new List<string>{ "Min", "Max" },
                                         colour = ColorToHexString(Color.Yellow)
                                     };

            return lineChart;
        }
    }
}