using System.Collections.Generic;

namespace Hygia.API.Widgets
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
        public string label { get; set; }
        public string value { get; set; }
    }

    public class Funnel
    {
        public Funnel()
        {
            item = new List<FunnelItem>();
        }

        public string type { get; set; }
        public string percentage { get; set; }
        public IList<FunnelItem> item { get; set; }
    }
}