using System;
using System.Collections.Generic;
using System.Linq;
using Hygia.LogicalMonitoring.Inspectors;

namespace Hygia.API.Models.LogicalMonitoring.MessageType
{
    public static class MessageTypeExtensions
    {
        public static IEnumerable<MessageType> ToOutputModels(this IEnumerable<Hygia.LogicalMonitoring.Handlers.MessageType> messageTypes)
        {
            return messageTypes.Select(ToOutputModel);
        }

        public static MessageType ToOutputModel(this Hygia.LogicalMonitoring.Handlers.MessageType messageType)
        {
            return new MessageType
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
}