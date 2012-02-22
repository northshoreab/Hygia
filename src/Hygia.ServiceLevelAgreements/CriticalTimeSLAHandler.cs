using System;
using System.Linq;
using Hygia.ServiceLevelAgreements.Domain;
using Hygia.ServiceLevelAgreements.Events;
using NServiceBus;
using Raven.Client;
using Hygia.PhysicalMonitoring.Events;

namespace Hygia.ServiceLevelAgreements
{
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
                    message.RegisteredEnvelope.ContainedMessages.Select(e => e.MessageTypeId).Contains(
                        x.MessageTypeId));

            foreach (var criticalTimeSLA in slas)
            {
                if(criticalTimeSLA.CriticalTime > message.RegisteredEnvelope.CriticalTime)
                {
                    _bus.Publish(new CriticalTimeSLAViolated
                                     {
                                         SLAId = criticalTimeSLA.Id,
                                         MessageTypeId = criticalTimeSLA.MessageTypeId,
                                         CriticalTimeSetting = criticalTimeSLA.CriticalTime,
                                         MessageCriticalTime = message.RegisteredEnvelope.CriticalTime.Value,
                                         TimeOfSLABreach = message.RegisteredEnvelope.ProcessingEnded.HasValue ? message.RegisteredEnvelope.ProcessingEnded.Value : DateTime.MinValue
                                     });
                }
            }
        }
    }
}