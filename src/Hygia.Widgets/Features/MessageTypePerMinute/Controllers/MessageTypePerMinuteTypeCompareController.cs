using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Hygia.Widgets.Features.MessageTypePerMinute.Domain;
using Hygia.Widgets.Features.MessageTypePerMinute.Models;
using Newtonsoft.Json;
using Raven.Client;

namespace Hygia.Widgets.Features.MessageTypePerMinute.Controllers
{
    public class MessageTypePerMinuteTypeCompareController
    {
        private readonly IDocumentSession _session;

        public MessageTypePerMinuteTypeCompareController(IDocumentSession session)
        {
            _session = session;
        }

        public string get_widgets_messagetypeperminutetypecompare_WidgetSettingId(MessageTypePerMinuteModel input)
        {
            //TODO: Change so that we get the settings from raven
            //var widgetSetting = _session.Load<MessageTypePerMinuteTypeCompare>(input.WidgetSettingId);

            IList<Guid> messageTypes = new List<Guid>
                                           {
                                               Guid.NewGuid(),
                                               Guid.NewGuid()
                                           };

            var widgetSetting = new MessageTypePerMinuteTypeCompare
                                    {
                                        ForMinutesInThePast = 10,
                                        MessageTypeId = messageTypes
                                    };

            DateTime from = DateTime.Now.AddMinutes(widgetSetting.ForMinutesInThePast * -1);

            //TODO: Change so that we call the raven index
            //var messageTypePerMinuteIndexData =
            //    _session.Query<MessageTypePerMinuteIndexData, PhysicalMonitoring.Index.MessageTypePerMinute>().Where(
            //        x => widgetSetting.MessageTypeId.Contains(x.MessageTypeId) && x.Minute >= from).ToList();

            var messageTypePerMinuteIndexData = new List<MessageTypePerMinuteIndexData>
                                                    {
                                                        new MessageTypePerMinuteIndexData
                                                            {
                                                                MessageTypeId = messageTypes.First(),
                                                                Minute = DateTime.Now.AddMinutes(-5),
                                                                Count = 10
                                                            },
                                                        new MessageTypePerMinuteIndexData
                                                            {
                                                                MessageTypeId = messageTypes[1],
                                                                Minute = DateTime.Now,
                                                                Count = 7
                                                            }
                                                    };

            var pieChart = new PieChart();

            foreach (var typePerMinuteIndexData in messageTypePerMinuteIndexData)
            {
                pieChart.Item.Add(new PieChartItem
                                      {
                                          Label = typePerMinuteIndexData.MessageTypeId.ToString(),
                                          Value = typePerMinuteIndexData.Count.ToString(CultureInfo.InvariantCulture)
                                      });
            }

            return JsonConvert.SerializeObject(pieChart);
        }
    }
}