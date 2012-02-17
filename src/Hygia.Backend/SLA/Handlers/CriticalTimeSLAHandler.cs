using System.Linq;
using Hygia.Backend.SLA.Domain;
using Hygia.Backend.SLA.Events;
using Hygia.PhysicalMonitoring.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.SLA.Handlers
{
    using PhysicalMonitoring.Events;

    public class CriticalTimeSLAHandler : IHandleMessages<EnvelopeRegistered>
    {
        private readonly IBus _bus;
        public IDocumentSession Session { get; set; }

        public CriticalTimeSLAHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(EnvelopeRegistered message)
        {
            var slas =
                Session.Load<CriticalTimeSLA>().Where(
                    x =>
                    message.RegisteredEnvelope.ContainedMessages.Select(e => e.MessageTypeId.ToString()).Contains(
                        x.MessageTypeId));

            foreach (var criticalTimeSLA in slas)
            {
                if(criticalTimeSLA.CriticalTime > message.RegisteredEnvelope.CriticalTime)
                {
                    _bus.Publish(new CriticalTimeSLAViolated
                                     {
                                         SLA = criticalTimeSLA,
                                         Envelope = message.RegisteredEnvelope
                                     });
                }
            }
        }
    }
}