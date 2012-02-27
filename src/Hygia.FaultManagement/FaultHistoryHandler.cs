namespace Hygia.FaultManagement
{
    using Operations.Events;
    using Raven.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Domain;
    using Events;
    using NServiceBus;
    using NServiceBus.Unicast.Subscriptions;
    using Headers = NServiceBus.Unicast.Monitoring.Headers;

    public class FaultHistoryHandler : IHandleMessages<FaultMessageReceived>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        public void Handle(FaultMessageReceived message)
        {
            var envelopeId = message.Headers["NServiceBus.OriginalId"].ToGuid();

            var timeOfFailure = message.Headers["NServiceBus.TimeSent"].ToUtcDateTime();

            if (message.Headers.ContainsKey("NServiceBus.TimeOfFailure"))
                timeOfFailure = message.Headers["NServiceBus.TimeOfFailure"].ToUtcDateTime();

            var messageTypes = message.MessageTypes().Select((messageType, ordinal) => new PhysicalMessage
                                                                                           {
                                                                                               MessageId = (envelopeId + ordinal.ToString()).ToGuid(),
                                                                                               MessageTypeId = messageType.TypeName.ToGuid()
                                                                                           }).ToList();
            var exception = new ExceptionInfo
                                {
                                    Message = message.Headers["NServiceBus.ExceptionInfo.Message"],
                                    Reason = message.Headers["NServiceBus.ExceptionInfo.Reason"],
                                    ExceptionType =
                                        message.Headers["NServiceBus.ExceptionInfo.ExceptionType"],
                                    Source = message.Headers["NServiceBus.ExceptionInfo.Source"],
                                    StackTrace = message.Headers["NServiceBus.ExceptionInfo.StackTrace"],
                                };

            var fault = new Fault
                            {
                                Id = envelopeId,
                                FaultEnvelopeId = message.FaultEnvelopeId.ToGuid(),
                                Body = message.Body,
                                Status = FaultStatus.New,
                                AssignedTo = Guid.Empty,
                                TimeOfFailure = timeOfFailure,
                                Exception = exception,
                                Endpoint = message.Headers["NServiceBus.FailedQ"],
                                EndpointId = message.Headers["NServiceBus.FailedQ"].ToGuid(),
                                Headers = message.Headers,
                                ContainedMessages = messageTypes,
                                History = new List<HistoryItem>
                                              {
                                                  new HistoryItem{TimeOfFailure = timeOfFailure,Exception = exception}
                                              }

                            };

            Session.Store(fault);

            Bus.Publish(new FaultRegistered
                             {
                                 FaultEnvelopeId = fault.FaultEnvelopeId,
                                 EnvelopeId = fault.Id,
                                 MessageTypes = messageTypes.Select(t => t.MessageTypeId).ToList()
                             });
        }
    }

    public static class FaultMessageReceivedExtensions
    {
        public static bool HasHeader(this FaultMessageReceived envelope, string header)
        {
            if (envelope.Headers == null)
                return false;

            return envelope.Headers.ContainsKey(header);
        }

        public static IEnumerable<MessageType> MessageTypes(this FaultMessageReceived transportMessageReceived)
        {
            var result = new List<MessageType>();

            if (!transportMessageReceived.HasHeader(Headers.EnclosedMessageTypes))
                return result;

            return transportMessageReceived.Headers[Headers.EnclosedMessageTypes].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                .Select(s => new MessageType(s));
        }
    }
}
