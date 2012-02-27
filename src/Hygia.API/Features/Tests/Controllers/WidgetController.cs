using Hygia.API.Features.Tests.Models;
using Hygia.API.Features.Tests.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;

namespace Hygia.API.Features.Tests.Controllers
{
    public class WidgetController
    {
        private readonly IDocumentSession _session;
        public const string OrderFunnel = "ORDERFUNNEL";
        public const string PieChart = "PIECHART";
        public const string LineChart = "LINECHART";
        public const string HighChartsLineChart = "HIGHCHARTSLINECHART";

        public WidgetController(IDocumentSession session)
        {
            _session = session;
        }

        public string get_widgets_test_WidgetSettingId(TestWidgetModel input)
        {
            switch (input.WidgetSettingId.ToUpper())
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