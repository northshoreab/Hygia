using System;
using System.Linq;

namespace Hygia.Backend.SLA.Domain
{
    public class CriticalTimeRule : Rule
    {
        public Guid MessageTypeId { get; set; } 
        public TimeSpan AlarmThreshold { get; set; }

        public CriticalTimeRule()
        {
            RuleExpression = () => Envelope.ContainedMessages.Any(x => x.MessageTypeId == MessageTypeId) &&
                                   Envelope.CriticalTime > AlarmThreshold;
        }
    }
}