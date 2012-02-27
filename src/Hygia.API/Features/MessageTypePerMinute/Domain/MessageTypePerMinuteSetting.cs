using System;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Hygia.API.Features.MessageTypePerMinute.Domain
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
        public Guid MessageTypeId { get; set; }
        public long Count { get; set; }
        public DateTime Minute { get; set; }
    }

    public class MessageTypePerMinute
    {
        public Guid Id { get; set; }
        public Guid MessageTypeId { get; set; }
        public long ForMinutesInThePast { get; set; }
        public HighChartsConfig HighChartsConfig { get; set; }   
    }
}