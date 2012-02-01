using Hygia.API.Models;
using Hygia.API.Testdata;
using Newtonsoft.Json;

namespace Hygia.API.Controllers
{
    public class TennantWidgetController
    {
        public const string OrderFunnel = "ORDERFUNNEL";
        public const string PieChart = "PIECHART";
        public const string LineChart = "LINECHART";

        public string get_dashboard_Tennant_Widget(TennantWidgetModel input)
        {
            switch (input.Widget.ToUpper())
            {
                case OrderFunnel:
                    return JsonConvert.SerializeObject(TestdataHelper.GetFunnel());
                case PieChart:
                    return JsonConvert.SerializeObject(TestdataHelper.GetPieChart());
                case LineChart:
                    return JsonConvert.SerializeObject(TestdataHelper.GetLineChart());
            }

            return "No widget found";
        }
    }
}