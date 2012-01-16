namespace Hygia.LaunchPad.LogicalMonitoring.Inspectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Core;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public class ServiceStructureInspector : IInspectEnvelopes
    {
        //for now we can only look at the message types since we don't know which handlers have fired for this message
        // to do this we need to add a client side inspection (message mutator) that finds all the handlers for this message
        public IEnumerable<object> Inspect(TransportMessage transportMessage)
        {
            var messageTypes = transportMessage.MessageTypes().ToList();

            if (!messageTypes.Any())
                yield break;

            foreach (var messageType in messageTypes)
            {
                var serviceName = DetermineServiveName(messageType);
                var serviceId = serviceName.ToGuid();
                yield return new RegisterLogicalService
                {
                    ServiceId = serviceId,
                    ServiceName = serviceName,
                };

                var bcName = DetermineBC(messageType);
                Guid bcId = Guid.Empty;

                if (!string.IsNullOrEmpty(bcName))
                {
                    bcId = bcName.ToGuid();

                    yield return new RegisterBusinessComponent
                    {
                        BusinessComponentId = bcId,
                        BusinessComponentName = bcName,
                        OwnedByService = serviceId,
                    };
                }
                    

                yield return new RegisterMessageOwner
                {
                    OwnedByService = serviceId,
                    OwnedByComponent = bcId,
                    MessageTypeId = messageType.TypeName.ToGuid()
                };

            }
        }

        string DetermineBC(MessageType messageType)
        {
            if (messageType.TypeName.Split('.').Count() > 2)
                return messageType.TypeName.Split('.').ElementAt(1);

            return null;
        }

        string DetermineServiveName(MessageType messageType)
        {
            return messageType.TypeName.Split('.').First();
        }
    }
}