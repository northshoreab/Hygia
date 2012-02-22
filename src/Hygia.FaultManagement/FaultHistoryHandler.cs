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

    public class FaultHistoryHandler:IHandleMessages<FaultMessageReceived>
    {
        private readonly IBus _bus;
        public IDocumentSession Session { get; set; }

        public FaultHistoryHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(FaultMessageReceived message)
        {
            var messageId = message.Headers["NServiceBus.OriginalId"].ToGuid();

            var timeOfFailure = message.Headers["NServiceBus.TimeSent"].ToUtcDateTime();

            if (message.Headers.ContainsKey("NServiceBus.TimeOfFailure"))
                timeOfFailure = message.Headers["NServiceBus.TimeOfFailure"].ToUtcDateTime();

            var messageTypes = message.MessageTypes().Select((messageType, ordinal) => new PhysicalMessage
                                                                                           {
                                                                                               MessageId = (messageId + ordinal.ToString()).ToGuid(),
                                                                                               MessageTypeId = messageType.TypeName.ToGuid()
                                                                                           }).ToList();

            var fault = new Fault
                            {
                                Id = messageId,
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

            _bus.Publish(new FaultRegistered
                             {
                                 Fault = fault
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
