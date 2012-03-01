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

    public class FaultMessageReceivedHandler : IHandleMessages<FaultMessageReceived>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        public void Handle(FaultMessageReceived message)
        {
            var envelopeId = message.Headers["NServiceBus.OriginalId"].ToGuid();

            var timeOfFailure = DetermineTimeOfFailure(message);

            var messageTypes = message.MessageTypes().Select((messageType, ordinal) => new PhysicalMessage
                                                                                           {
                                                                                               MessageId = (envelopeId + ordinal.ToString()).ToGuid(),
                                                                                               MessageTypeId = messageType.TypeName.ToGuid()
                                                                                           }).ToList();
            var exception = ExtractExceptionInfo(message);
            var fault = Session.Load<Fault>(envelopeId);
            if (fault == null)
                fault = new Fault
                                {
                                    Id = envelopeId,
                                    Status = FaultStatus.New,
                                    Retries = 0,
                                    AssignedTo = Guid.Empty,
                                    Endpoint = message.Headers["NServiceBus.FailedQ"],
                                    EndpointId = message.Headers["NServiceBus.FailedQ"].ToGuid(),
                                };
            else
            {
                fault.Status = FaultStatus.RepetedFailures;
                fault.Retries++;
            }

            fault.FaultEnvelopeId = message.FaultEnvelopeId;
            fault.Headers = message.Headers;
            fault.ContainedMessages = messageTypes;
                                
            fault.TimeOfFailure = timeOfFailure;
            fault.Exception = exception;
                                   
            fault.Body = message.Body;
            fault.History.Add(new HistoryItem{Time = timeOfFailure,Status = "Failed with exception - " +exception.Message});
                                              
                               
            Session.Store(fault);

            Bus.Publish(new FaultRegistered
                             {
                                 EnvelopeId = fault.Id,
                                 MessageTypes = messageTypes.Select(t => t.MessageTypeId).ToList()
                             });
        }

        static DateTime DetermineTimeOfFailure(FaultMessageReceived message)
        {
            var timeOfFailure = message.Headers["NServiceBus.TimeSent"].ToUtcDateTime();

            if (message.Headers.ContainsKey("NServiceBus.TimeOfFailure"))
                timeOfFailure = message.Headers["NServiceBus.TimeOfFailure"].ToUtcDateTime();
            return timeOfFailure;
        }

        static ExceptionInfo ExtractExceptionInfo(FaultMessageReceived message)
        {
            var exception = new ExceptionInfo
                                {
                                    Message = message.Headers["NServiceBus.ExceptionInfo.Message"],
                                    Reason = message.Headers["NServiceBus.ExceptionInfo.Reason"],
                                    ExceptionType =
                                        message.Headers["NServiceBus.ExceptionInfo.ExceptionType"],
                                    Source = message.Headers["NServiceBus.ExceptionInfo.Source"],
                                    StackTrace = message.Headers["NServiceBus.ExceptionInfo.StackTrace"],
                                };
            return exception;
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
