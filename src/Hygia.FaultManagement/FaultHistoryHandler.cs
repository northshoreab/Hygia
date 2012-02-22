using System;
using System.Collections.Generic;
using System.Linq;
using Hygia.Core;
using Hygia.FaultManagement.Domain;
using Hygia.FaultManagement.Events;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;
using Headers = NServiceBus.Unicast.Monitoring.Headers;

namespace Hygia.FaultManagement
{
    //using NServiceBus;
    using Operations.Events;
    using Raven.Client;

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

            var fault = new Fault
                            {
                                Id = envelopeId,
                                FaultEnvelopeId = message.FaultEnvelopeId.ToGuid(),
                                Body = message.Body,
                                TimeOfFailure = timeOfFailure,
                                Exception = new ExceptionInfo
                                                {
                                                    Message = message.Headers["NServiceBus.ExceptionInfo.Message"],
                                                    Reason = message.Headers["NServiceBus.ExceptionInfo.Reason"],
                                                    ExceptionType =
                                                        message.Headers["NServiceBus.ExceptionInfo.ExceptionType"],
                                                    Source = message.Headers["NServiceBus.ExceptionInfo.Source"],
                                                    StackTrace = message.Headers["NServiceBus.ExceptionInfo.StackTrace"],
                                                },
                                Endpoint = message.Headers["NServiceBus.FailedQ"],
                                EndpointId = message.Headers["NServiceBus.FailedQ"].ToGuid(),
                                Headers = message.Headers,
                                ContainedMessages = messageTypes

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
