using System;
using System.Collections.Generic;

namespace Hygia.API.Features.MessageTypePerMinute.Domain
{
    public class MessageTypePerMinuteTypeCompare
    {
        public Guid Id { get; set; }
        public IList<Guid> MessageTypeId { get; set; }
        public long ForMinutesInThePast { get; set; }
    }
}