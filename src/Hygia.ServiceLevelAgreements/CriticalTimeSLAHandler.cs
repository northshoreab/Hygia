using NServiceBus;
using Raven.Client;

namespace Hygia.ServiceLevelAgreements
{
    using Domain;
    using Events;
    using Operations.Events;

    public class CriticalTimeSLAHandler : IHandleMessages<AuditMessageReceived>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        public void Handle(AuditMessageReceived message)
        {
            var environmentSLA = Session.Load<EnvironmentSLA>("Environment/ServiceLevelAgreement");

            if (environmentSLA == null)
                return;

            foreach(var applicativeMessage in message.Headers.MessageTypes())
            {
                var criticalTimeSLA = environmentSLA.DefaultCriticalTimeSLA;

                var messageSpecificSLA = Session.Load<MessageTypeSLA>(applicativeMessage.MessageTypeId());

                if (messageSpecificSLA != null)
                    criticalTimeSLA = messageSpecificSLA.CriticalTimeSLA;
                var criticalTime = message.Headers.CriticalTime();

                if (criticalTimeSLA > criticalTime)
                {
                    Bus.Publish(new CriticalTimeSLAViolated
                                     {
                                         MessageTypeId = applicativeMessage.MessageTypeId(),
                                         ActiveSLA = criticalTimeSLA,
                                         ActualCriticalTime = criticalTime,
                                         TimeOfSLABreach = message.Headers.ProcessingEnded()
                                     });
                }
            }
        }
    }
}