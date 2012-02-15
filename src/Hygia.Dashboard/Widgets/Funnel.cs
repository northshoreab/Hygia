using System.Collections.Generic;

namespace Hygia.Widgets.Widgets
{
    public static class FunnelType
    {
        public const string Reverse = "reverse";
        public const string Standard = "standard";
    }

    public static class FunnelPercentage
    {
        public const string Hide = "hide";
        public const string Show = "show";
    }

    public class FunnelItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class Funnel
    {
        public Funnel()
        {
            Item = new List<FunnelItem>();
        }

        public string Type { get; set; }
        public string Percentage { get; set; }
        public IList<FunnelItem> Item { get; set; }
    }
}