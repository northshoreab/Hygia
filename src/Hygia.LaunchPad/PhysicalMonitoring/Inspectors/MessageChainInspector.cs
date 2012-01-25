namespace Hygia.LaunchPad.PhysicalMonitoring.Inspectors
{
    using System;
    using System.Linq;
    using AuditProcessing.Events;
    using Commands;
    using Domain;
    using NServiceBus;

    public class MessageChainInspector : IHandleMessages<AuditMessageReceived>
    {
        readonly IBus bus;

        public MessageChainInspector(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(AuditMessageReceived auditMessage)
        {
            var messages = auditMessage.MessageTypes()
                .Select((messageType, ordinal) => new PhysicalMessage
                            {
                                MessageId = (auditMessage.MessageId + ordinal.ToString()).ToGuid(),
                                MessageTypeId = messageType.TypeName.ToGuid()
                            }).ToList();


            var previousEnvelopeId = auditMessage.PreviousEnvelopeId();

            foreach (var message in messages)
            {
                if (previousEnvelopeId == Guid.Empty)
                    bus.Send(new StartMessageChain
                                 {
                                     MessageChainId = Guid.NewGuid(),
                                     MessageId = message.MessageId,
                                     EnvelopeId = auditMessage.EnvelopeId()
                                 });
                else
                {
                    bus.Send(new AppendMessageToChain
                                 {
                                     MessageId = message.MessageId,
                                     EnvelopeId = auditMessage.EnvelopeId(),
                                     PreviousEnvelopeId = previousEnvelopeId
                                 });
                }
            }
        }

    }
}