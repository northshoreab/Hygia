namespace Hygia.LaunchPad.LogicalMonitoring.Inspectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AuditProcessing.Events;
    using Commands;
    using Core;
    using NServiceBus;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public class ServiceStructureInspector : IHandleMessages<AuditMessageReceived>
    {
        IBus bus;

        public ServiceStructureInspector(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(AuditMessageReceived transportMessage)
        {
            var messageTypes = transportMessage.MessageTypes().ToList();

            if (!messageTypes.Any())
                return;

            foreach (var messageType in messageTypes)
            {
                var serviceName = DetermineServiveName(messageType);
                var serviceId = serviceName.ToGuid();
                bus.Send(new RegisterLogicalService
                {
                    ServiceId = serviceId,
                    ServiceName = serviceName,
                });

                var bcName = DetermineBC(messageType);
                Guid bcId = Guid.Empty;

                if (!string.IsNullOrEmpty(bcName))
                {
                    bcId = bcName.ToGuid();

                    bus.Send(new RegisterBusinessComponent
                    {
                        BusinessComponentId = bcId,
                        BusinessComponentName = bcName,
                        OwnedByService = serviceId,
                    });
                }
                    

                bus.Send(new RegisterMessageOwner
                {
                    OwnedByService = serviceId,
                    OwnedByComponent = bcId,
                    MessageTypeId = messageType.TypeName.ToGuid()
                });

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