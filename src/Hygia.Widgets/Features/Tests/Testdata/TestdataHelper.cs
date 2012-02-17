using System;
using System.Collections.Generic;
using System.Drawing;
using Hygia.Notifications.Domain;

namespace Hygia.Widgets.Features.Tests.Testdata
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

        public static IList<Notification> Notifications { get; set; }

        static TestdataHelper()
        {
            Notifications = new List<Notification>
                                {
                                    new CriticalTimeNotification("OrderPaid", new TimeSpan(0,0,0,2,123), new TimeSpan(0,0,0,2,0), "OrderPaid"),
                                    new Notification
                                        {
                                            Author = new Author
                                                         {
                                                             Name = "Sales department",
                                                             Email = "sales@test.se"
                                                         },
                                            Description = "The Dublin coffe house sets new record for moccas sold!",
                                            Title = "New record for Moccas sold",
                                            NotificationDate = DateTime.Now.AddDays(-1).AddHours(-3),
                                            Summary = "The Dublin coffe house sets new record for moccas sold"
                                        },
                                    //new BusinessProcessCompletionTime("Order paid", "Order delivered", "Coffee delivery process", new TimeSpan(0,10,0))
                                };
        }


        public static Funnel GetFunnel()
        {
            var funnel = new Funnel {Percentage = FunnelPercentage.Show, Type = FunnelType.Reverse};

            funnel.Item.Add(new FunnelItem
                                {
                                    Label = "Order placed",
                                    Value = "502"
                                });

            funnel.Item.Add(new FunnelItem
                                {
                                    Label = "Order paid",
                                    Value = "311"
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

        public static HighChartsConfig GetHighChart()
        {
            return new HighChartsConfig
                       {
                           DefaultSeriesType = HighChartsSeriesTypes.Line,
                           Legend = new HighChartsLegend
                                        {
                                            Align = HighChartsAlign.Right,
                                            BorderWidth = 0,
                                            X = -10,
                                            Y = 100,
                                            Layout = HighChartsLayout.Vertical,
                                            VerticalAlign = HighChartsAlign.Top
                                        },
                           MarginBottom = 25,
                           MarginRight = 130,
                           RenderTo = HighChartsRenderTo.Container,
                           Series =
                               new List<HighChartsData>
                                   {
                                       new HighChartsData
                                           {
                                               Name = "This year",
                                               Data = new List<double> {200, 300, 320, 250, 600}
                                           },
                                       new HighChartsData
                                           {
                                               Name = "Last year",
                                               Data = new List<double> {420, 323, 320, 276, 541}
                                           }

                                   },
                           SeriesType = HighChartsSeriesTypes.Line,
                           SubTitle = new HighChartsTitle{ Text = "This year compared to last year", xPos = -20},
                           Title = new HighChartsTitle
                                       {
                                           Text = "Order history",
                                           xPos = -20
                                       },
                           xAxis = new HighChartsXAxis
                                       {
                                           Categories = new List<string> {"Jan", "Feb", "Mar", "Apr", "May"}
                                       },
                           yAxis = new HighChartsYAxis
                                       {
                                           Title = new HighChartsTitle {Text = "Orders"},
                                           PlotLines = new List<HighChartsPlotLine>
                                                           {
                                                               new HighChartsPlotLine
                                                                   {
                                                                       Color = "#808080",
                                                                       Value = 0,
                                                                       Width = 1
                                                                   }
                                                           }
                                       }
                       };
        }

        public static LineChart GetLineChart()
        {
            var lineChart = new LineChart();

            lineChart.Item.Add("500");
            lineChart.Item.Add("600");
            lineChart.Item.Add("531");
            lineChart.Settings = new LineChartSettings
                                     {
                                         Axisx = new List<string>{ "Jun", "Jul", "Aug" },
                                         Axisy = new List<string>{ "Min", "Max" },
                                         Colour = ColorToHexString(Color.Yellow)
                                     };

            return lineChart;
        }
    }
}