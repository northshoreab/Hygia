namespace Hygia.LaunchPad.LogicalMonitoring.Inspectors
{
    using System.Linq;
    using AuditProcessing.Events;
    using Commands;
    using NServiceBus;

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
                   bus.Send(new RegisterAutonomousComponent
                                 {
                                     AutonomousComponentId = ServiceStructureConventions.AutonomousComponentId(autonomousComponent),
                                     ServiceId = ServiceStructureConventions.ServiceId(autonomousComponent),
                                     AutonomousComponentName = ServiceStructureConventions.AutonomousComponentName(autonomousComponent),
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