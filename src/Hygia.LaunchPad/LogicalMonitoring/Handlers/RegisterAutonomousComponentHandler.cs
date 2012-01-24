namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
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

            
            var existingAc = service.AutonomousComponents.SingleOrDefault(ac => ac.Id == message.AutonomousComponentId);
            if(existingAc == null)
            {
              service.AutonomousComponents.Add(new AutonomousComponent
                         {
                             Id = message.AutonomousComponentId,
                             Name = message.AutonomousComponentName,
                             Versions = new List<string>{message.Version}
                         });
            }
            else
            {
                if(existingAc.Versions == null)
                    existingAc.Versions = new List<string>();

                if(!existingAc.Versions.Contains(message.Version))
                    existingAc.Versions.Add(message.Version);
            }
            session.Store(service);
        }
    }
}