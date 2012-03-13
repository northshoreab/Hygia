using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using Hygia.LogicalMonitoring.Handlers;
using Hygia.LogicalMonitoring.Inspectors;
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
                                    MessageTypeId = messageType.Id,
                                    Type = messageType.Type,
                                    ConsumedByACs = messageType.ConsumedByACs == null ? "" : string.Join(", ", messageType.ConsumedByACs),
                                    Intent = Enum.GetName(typeof(MessageIntent), messageType.Intent),
                                    PreceedingMessageTypes = messageType.PreceedingMessageTypes == null ? "" : string.Join(", ", messageType.PreceedingMessageTypes),
                                    ProducedByACs = messageType.ProducedByACs == null ? "" : string.Join(", ", messageType.ProducedByACs),
                                    Versions = messageType.Versions == null ? "" : string.Join(", ", messageType.Versions)
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
        public string Type { get; set; }
        public string Intent { get; set; }
        public string Versions { get; set; }
        public string ConsumedByACs { get; set; }
        public string ProducedByACs { get; set; }
        public string PreceedingMessageTypes { get; set; }
    }
}
