using System;
using System.Collections.Generic;
using System.Linq;
using Hygia.Widgets.Domain;
using Hygia.Widgets.Models;
using Hygia.Widgets.Testdata;
using Hygia.Widgets.Widgets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.Widgets.Controllers
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

        public string get_widgets_messagetypeperminute_WidgetSettingId(WidgetModel input)
        {
            //TODO: Change so that we get the settings from raven
            //var widgetSetting = _session.Load<MessageTypePerMinute>(input.WidgetSettingId);

            var widgetSetting = new MessageTypePerMinute
                                    {
                                        HighChartsConfig = TestdataHelper.GetHighChart(),
                                        ForMinutesInThePast = 10
                                    };

            DateTime from = DateTime.Now.AddMinutes(widgetSetting.ForMinutesInThePast * -1);

            //TODO: Change so that we call the raven index
            //var messageTypePerMinuteIndexData =
            //    _session.Query<MessageTypePerMinuteIndexData, MessageTypePerMinuteIndex>().Where(
            //        x => x.MessageTypeId == widgetSetting.MessageTypeId && x.Minute >= from).ToList();

            var messageTypePerMinuteIndexData = new List<MessageTypePerMinuteIndexData>
                                                    {
                                                        new MessageTypePerMinuteIndexData
                                                            {
                                                                Minute = DateTime.Now.AddMinutes(-5),
                                                                Count = 10
                                                            },
                                                        new MessageTypePerMinuteIndexData
                                                            {
                                                                Minute = DateTime.Now,
                                                                Count = 7
                                                            }
                                                    };


            widgetSetting.HighChartsConfig.Series.Add(new HighChartsData
                                                          {
                                                              Data =
                                                                  messageTypePerMinuteIndexData.Select(
                                                                      x => (double) x.Count).ToList(),
                                                              Name =
                                                                  "Message count last " +
                                                                  widgetSetting.ForMinutesInThePast +
                                                                  " minutes"
                                                          });

            widgetSetting.HighChartsConfig.xAxis.Categories =
                messageTypePerMinuteIndexData.Select(x => x.Minute.ToShortTimeString()).ToList();

            return JsonConvert.SerializeObject(widgetSetting.HighChartsConfig);
        }

        public string get_widgets_test_WidgetSettingId(WidgetModel input)
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