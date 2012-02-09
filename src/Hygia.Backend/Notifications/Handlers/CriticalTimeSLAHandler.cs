using System.Collections.Generic;
using System.Linq;
using Hygia.Backend.Notifications.Domain;
using Hygia.Backend.Notifications.Events;
using Hygia.PhysicalMonitoring.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.Notifications.Handlers
{
    public class CriticalTimeSLAHandler : IHandleMessages<EnvelopeRegistered>
    {
        private readonly IBus _bus;
        public IDocumentSession Session { get; set; }

        private readonly IList<CriticalTimeSLA> _slas;

        public CriticalTimeSLAHandler(IBus bus)
        {
            _bus = bus;
            _slas = Session.Load<CriticalTimeSLA>();
        }

        public void Handle(EnvelopeRegistered message)
        {
            foreach (var criticalTimeSLA in _slas)
            {
                if(message.RegisteredEnvelope.ContainedMessages.Any(x => x.MessageTypeId == criticalTimeSLA.MessageTypeId))
                {
                    if (criticalTimeSLA.AlarmThreshold > message.RegisteredEnvelope.CriticalTime)
                        _bus.Publish(new CriticalTimeSLAViolated
                                         {
                                             SLAId = criticalTimeSLA.Id,
                                             MessageTypeId = criticalTimeSLA.MessageTypeId,
                                             ActualValue = message.RegisteredEnvelope.CriticalTime.Value
                                         });

                    if (criticalTimeSLA.WarnThreshold > message.RegisteredEnvelope.CriticalTime)
                        _bus.Publish(new CriticalTimeSLAWarning
                                         {
                                             SLAId = criticalTimeSLA.Id,
                                             MessageTypeId = criticalTimeSLA.MessageTypeId,
                                             ActualValue = message.RegisteredEnvelope.CriticalTime.Value
                                         });
                }
            }
        }
    }
}
