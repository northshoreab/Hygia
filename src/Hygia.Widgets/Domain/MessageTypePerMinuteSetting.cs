using System;
using Hygia.Widgets.Widgets;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Hygia.Widgets.Domain
{
    public class MessageTypePerMinuteIndex : AbstractIndexCreationTask
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            throw new NotImplementedException();
        }
    }

    public class MessageTypePerMinuteIndexData
    {
        public string MessageTypeId { get; set; }
        public long Count { get; set; }
        public DateTime Minute { get; set; }
    }

    public class MessageTypePerMinute
    {
        public string Id { get; set; }
        public string MessageTypeId { get; set; }
        public long ForMinutesInThePast { get; set; }
        public HighChartsConfig HighChartsConfig { get; set; }   
    }
}