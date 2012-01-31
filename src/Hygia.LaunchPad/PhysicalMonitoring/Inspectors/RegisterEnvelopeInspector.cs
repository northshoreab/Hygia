namespace Hygia.LaunchPad.PhysicalMonitoring.Inspectors
{
    using System;
    using System.Linq;
    using AuditProcessing.Events;
    using Commands;
    using Domain;
    using NServiceBus;
    using Headers = NServiceBus.Unicast.Monitoring.Headers;

    public class RegisterEnvelopeInspector : IHandleMessages<AuditMessageReceived>
    {
        public IBus Bus { get; set; }

        public void Handle(AuditMessageReceived auditMessage)
        {
            var messages = auditMessage.MessageTypes()
                .Select((messageType,ordinal) =>new PhysicalMessage
                            {
                                MessageId = (auditMessage.MessageId + ordinal.ToString()).ToGuid(),
                                MessageTypeId = messageType.TypeName.ToGuid()
                            }).ToList();


        
            var command = new RegisterEnvelope
                              {
                                  EnvelopeId = auditMessage.EnvelopeId(),
                                  CorrelatedEnvelopeId = auditMessage.CorrelationId(),
                                  ParentEnvelopeId = auditMessage.PreviousEnvelopeId(),
                                  Messages = messages
                              };
            
            if(auditMessage.Headers.ContainsKey(Headers.TimeSent))
                command.TimeSent = auditMessage.Headers[Headers.TimeSent].ToUtcDateTime();

            if (auditMessage.Headers.ContainsKey(Headers.ProcessingStarted))
                command.ProcessingStarted = auditMessage.Headers[Headers.ProcessingStarted].ToUtcDateTime();

            if (auditMessage.Headers.ContainsKey(Headers.ProcessingEnded))
                command.ProcessingEnded = auditMessage.Headers[Headers.ProcessingEnded].ToUtcDateTime();
           
            Bus.Send(command);
        }

    }
}