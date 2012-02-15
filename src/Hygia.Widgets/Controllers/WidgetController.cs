using Hygia.Widgets.Models;
using Hygia.Widgets.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hygia.Widgets.Controllers
{
    public class WidgetController
    {
        public const string OrderFunnel = "ORDERFUNNEL";
        public const string PieChart = "PIECHART";
        public const string LineChart = "LINECHART";
        public const string HighChartsLineChart = "HIGHCHARTSLINECHART";

        public string get_widgets_WidgetId(WidgetModel input)
        {
            switch (input.WidgetId.ToUpper())
            {
                case OrderFunnel:
                    return JsonConvert.SerializeObject(TestdataHelper.GetFunnel(),Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver()});
                case PieChart:
                    return JsonConvert.SerializeObject(TestdataHelper.GetPieChart(), Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                case LineChart:
                    return JsonConvert.SerializeObject(TestdataHelper.GetLineChart(), Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                case HighChartsLineChart:
                    return TestdataHelper.GetHighChart().ToString();
            }

            return "No widget found";
        }
    }
}