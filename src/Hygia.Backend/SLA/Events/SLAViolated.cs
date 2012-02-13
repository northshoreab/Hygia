using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;

namespace Hygia.Backend.SLA.Events
{
    public class SLAViolated : IMessage
    {
        public Domain.SLA SLA { get; set; }
        public Envelope Envelope { get; set; }
    }
}