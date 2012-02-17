using System;

namespace Hygia.Widgets.Features.MessageTypePerMinute.Domain
{
    public class MessageTypePerMinuteTypeCompare
    {
        public string Id { get; set; }
        public string MessageTypeId { get; set; }
        public long ForMinutesInThePast { get; set; }
        public HighChartsConfig HighChartsConfig { get; set; }   
    }
}