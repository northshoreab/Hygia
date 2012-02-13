namespace Hygia.LogicalMonitoring.Inspectors
{
    using System;
    using System.Linq;
    using Commands;
    using NServiceBus;
    using Operations.Events;

    public class ServiceStructureInspector : IHandleMessages<AuditMessageReceived>
    {
        readonly IBus bus;

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
                var serviceName = ServiceStructureConventions.ServiceName(messageType.TypeName);
                var serviceId = ServiceStructureConventions.ServiceId(messageType.TypeName);

                bus.Send(new RegisterLogicalService
                {
                    ServiceId = serviceId,
                    ServiceName = serviceName,
                });

                var bcName = ServiceStructureConventions.BusinessComponentName(messageType.TypeName);
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

    }
}