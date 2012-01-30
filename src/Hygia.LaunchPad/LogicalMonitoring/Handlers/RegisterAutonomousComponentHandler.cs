namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System.Collections.Generic;
    using Commands;
    using NServiceBus;
    using Raven.Client;

    public class RegisterAutonomousComponentHandler : IHandleMessages<RegisterAutonomousComponent>
    {
        readonly IDocumentSession session;

        public RegisterAutonomousComponentHandler(IDocumentSession session)
        {
            this.session = session;
        }

        public void Handle(RegisterAutonomousComponent message)
        {
            var service = session.Load<ServiceStructure>(message.ServiceId.ToString());

            if (service == null)
                service = new ServiceStructure { Id = message.ServiceId.ToString() };

            if(service.AutonomousComponents == null)
                service.AutonomousComponents = new List<AutonomousComponent>();

            var ac = new AutonomousComponent
                         {
                             Id = message.AutonomousComponentId,
                             Name = message.AutonomousComponentName
                         };

            if (service.AutonomousComponents.Contains(ac))
                service.AutonomousComponents.Remove(ac);

            service.AutonomousComponents.Add(ac);

            session.Store(service);
        }
    }
}