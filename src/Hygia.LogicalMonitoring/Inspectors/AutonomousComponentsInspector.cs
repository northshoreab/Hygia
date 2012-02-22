using Hygia.Core;

namespace Hygia.LogicalMonitoring.Inspectors
{
    using System.Linq;
    using Commands;
    using NServiceBus;
    using Operations.Events;

    public class AutonomousComponentsInspector : IHandleMessages<AuditMessageReceived>
    {
        readonly IBus bus;

        public AutonomousComponentsInspector(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(AuditMessageReceived auditMessage)
        {
            var messageTypes = auditMessage.MessageTypes().ToList();

            if (!messageTypes.Any())
                return;

            foreach (var messageType in messageTypes)
            {
                var autonomousComponentsThatHandledThisMessage = auditMessage.GetPipelineInfoFor(messageType).ToList();



                foreach (var autonomousComponent in autonomousComponentsThatHandledThisMessage)
                {
                    var typeName = autonomousComponent.Split(',').First();
                    var version = autonomousComponent.Split(',').Single(s => s.StartsWith(" Version")).Split('=').Last();
                    bus.Send(new RegisterAutonomousComponent
                                 {
                                     AutonomousComponentId = ServiceStructureConventions.AutonomousComponentId(typeName),
                                     ServiceId = ServiceStructureConventions.ServiceId(typeName),
                                     AutonomousComponentName = ServiceStructureConventions.AutonomousComponentName(typeName),
                                     Version = version
                                 });
                }

                if (autonomousComponentsThatHandledThisMessage.Any())
                    bus.Send(new RegisterMessageConsumers
                    {
                        MessageTypeId = messageType.TypeName.ToGuid(),
                        ConsumedBy = autonomousComponentsThatHandledThisMessage.Select(ServiceStructureConventions.AutonomousComponentId).ToList()
                    });
            }
        }

    }
}