using Hygia.API.Models;
using Hygia.API.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace Hygia.API.Controllers
{
    public class DashboardWidgetController
    {
        public const string OrderFunnel = "ORDERFUNNEL";
        public const string PieChart = "PIECHART";
        public const string LineChart = "LINECHART";
        public const string HighChartsLineChart = "HIGHCHARTSLINECHART";

        public string get_Tennant_dashboard_Widget(DashboardWidgetModel input)
        {
            switch (input.Widget.ToUpper())
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