using System.Collections.Generic;
using System.Linq;

namespace Hygia.Widgets.Widgets
{
    public static class HighChartsRenderTo
    {
        public const string Container = "container";
    }
    public static class HighChartsLayout
    {
        public const string Vertical = "vertical";
    }
    public static class HighChartsAlign
    {
        public const string Top = "top";
        public const string Right = "right";
    }
    public static class HighChartsSeriesTypes
    {
        public const string Line = "line";
    }

    public class HighChartsData
    {
        public HighChartsData()
        {
            Data = new List<double>();
        }

        public string Name { get; set; }
        public IList<double> Data { get; set; }
    }

    public class HighChartsTitle
    {
        public string Text { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }
    }

    public class HighChartsPlotLine
    {
        public int Value { get; set; }
        public int Width { get; set; }
        public string Color { get; set; }
    }

    public class HighChartsLegend
    {
        public string Layout { get; set; }
        public string Align { get; set; }
        public string VerticalAlign { get; set; }
        public int X { get; set;}
        public int Y { get; set; }
        public int BorderWidth { get; set; }
    }

    public class HighChartsXAxis
    {
        public HighChartsXAxis()
        {
            Categories = new List<string>();
        }

        public IList<string> Categories { get; set; }
    }

    public class HighChartsYAxis
    {
        public HighChartsYAxis()
        {
            PlotLines = new List<HighChartsPlotLine>();
        }

        public HighChartsTitle Title { get; set; }
        public IList<HighChartsPlotLine> PlotLines { get; set; }
    }

    public class HighChartsConfig
    {
        public HighChartsConfig()
        {
            Series = new List<HighChartsData>();
        }

        public HighChartsTitle Title { get; set; }
        public HighChartsTitle SubTitle { get; set; }
        public string SeriesType { get; set; }
        public HighChartsXAxis xAxis { get; set; }
        public HighChartsYAxis yAxis { get; set; }
        public IList<HighChartsData> Series { get; set; }
        public HighChartsLegend Legend { get; set; }
        public string RenderTo { get; set; }
        public string DefaultSeriesType { get; set; }
        public int MarginRight { get; set; }
        public int MarginBottom { get; set; }

        public override string ToString()
        {
            string config = "{ chart: { renderTo: '" + RenderTo + "', defaultSeriesType: '" + DefaultSeriesType +
                   "', marginRight: " + MarginRight + ", marginBottom: " + MarginBottom + "}, title: { text: '" +
                   Title.Text + "', x: " + Title.xPos + " }, subtitle: { text: '" + SubTitle.Text + "', x: " +
                   SubTitle.xPos + " }, xAxis: { categories: [";

            config = xAxis.Categories.Aggregate(config, (current, category) => current + "'" + category + "', ");

            config = config.Substring(0, config.Length - 2);

            config = config + "]}, yAxis: { title: { text: '" + yAxis.Title.Text + "'}, plotLines: [";

            config = yAxis.PlotLines.Aggregate(config, (current, highChartsPlotLine) => current + "{ value: " + highChartsPlotLine.Value + ", width: " + highChartsPlotLine.Width + ", color: '" + highChartsPlotLine.Color + "'}, ");

            config = config.Substring(0, config.Length - 2);

            config = config + "]}, " + " tooltip: { formatter: function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y; } }, legend: { layout: '" + Legend.Layout + "', align: '" + Legend.Align +
                     "', verticalAlign: '" + Legend.VerticalAlign + "', x: " + Legend.X + ", y: " + Legend.Y +
                     ", borderWidth: " + Legend.BorderWidth + "}, series: [";

            foreach (var highChartsData in Series)
            {
                config = config + "{ name: '" + highChartsData.Name + "', data: [";

                foreach (var d in highChartsData.Data)
                {
                    config = config + d + ", ";
                }

                config = config.Substring(0, config.Length - 2);

                config = config + "]}, ";
            }

            config = config.Substring(0, config.Length - 2);

            config = config + "]}";
            return config;
        }
    }
}