using Hygia.Backend.SLA.Domain;
using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;

namespace Hygia.Backend.SLA.Events
{
    public interface ISLAMessage : IMessage { }
    public class CriticalTimeSLAViolated : ISLAMessage
    {
        public CriticalTimeSLA SLA { get; set; }
        public Envelope Envelope { get; set; }
    }
}