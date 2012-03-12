using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using Hygia.LogicalMonitoring.Handlers;
using Raven.Client;

namespace Hygia.LogicalMonitoring.Api
{
    public class MessageTypesController
    {
        public IDocumentSession Session { get; set; }

        [JsonEndpoint]
        public IEnumerable<MessageTypeOutputModel> get_messagetypes()
        {
            return Session.Query<MessageType>().ToList().ToOutputModels();
        }

        [JsonEndpoint]
        public MessageTypeOutputModel get_messagetypes_MessageTypeId(MessageTypeInputModel model)
        {
            return Session.Load<MessageType>(model.MessageTypeId).ToOutputModel();
        }
    }

    public static class FaultEnvelopeViewModelExtensions
    {
        public static IEnumerable<MessageTypeOutputModel> ToOutputModels(this IEnumerable<MessageType> messageTypes)
        {
            return messageTypes.Select(messageType => messageType.ToOutputModel());
        }

        public static MessageTypeOutputModel ToOutputModel(this MessageType messageType)
        {
            return new MessageTypeOutputModel
                                {
                                    MessageTypeId = messageType.Id
                                };
        }
    }

    public class MessageTypeInputModel
    {
        public Guid MessageTypeId { get; set; }
    }

    public class MessageTypeOutputModel
    {
        public Guid MessageTypeId { get; set; }
    }
}
