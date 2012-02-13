using Hygia.Backend.SLA.Domain;
using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;

namespace Hygia.Backend.SLA.Events
{
    public class CriticalTimeSLAViolated : IMessage
    {
        public CriticalTimeSLA SLA { get; set; }
        public Envelope Envelope { get; set; }
    }
}