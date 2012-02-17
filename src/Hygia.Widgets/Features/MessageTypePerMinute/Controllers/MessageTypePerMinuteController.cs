using System;
using System.Collections.Generic;
using System.Linq;
using Hygia.Widgets.Features.MessageTypePerMinute.Domain;
using Hygia.Widgets.Features.MessageTypePerMinute.Models;
using Hygia.Widgets.Features.Tests.Testdata;
using Newtonsoft.Json;
using Raven.Client;

namespace Hygia.Widgets.Features.MessageTypePerMinute.Controllers
{
    public class MessageTypePerMinuteController
    {
        private readonly IDocumentSession _session;

        public MessageTypePerMinuteController(IDocumentSession session)
        {
            _session = session;
        }

        public string get_widgets_messagetypeperminute_WidgetSettingId(MessageTypePerMinuteModel input)
        {
            //TODO: Change so that we get the settings from raven
            //var widgetSetting = _session.Load<MessageTypePerMinute>(input.WidgetSettingId);

            var widgetSetting = new Domain.MessageTypePerMinute
                                    {
                                        HighChartsConfig = TestdataHelper.GetHighChart(),
                                        ForMinutesInThePast = 10
                                    };

            DateTime from = DateTime.Now.AddMinutes(widgetSetting.ForMinutesInThePast * -1);

            //TODO: Change so that we call the raven index
            //var messageTypePerMinuteIndexData =
            //    _session.Query<MessageTypePerMinuteIndexData, PhysicalMonitoring.Index.MessageTypePerMinute>().Where(
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
    }
}