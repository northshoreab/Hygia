namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using Commands;
    using NServiceBus;
    using Raven.Client;

    public class RegisterLogicalServiceHandler:IHandleMessages<RegisterLogicalService>
    {
        readonly IDocumentSession session;

        public RegisterLogicalServiceHandler(IDocumentSession session)
        {
            this.session = session;
        }

        public void Handle(RegisterLogicalService message)
        {
            var service = session.Load<ServiceStructure>(message.ServiceId.ToString());

            if (service == null)
                service = new ServiceStructure {Id = message.ServiceId.ToString()};

            service.Name = message.ServiceName;

            session.Store(service);
        }
    }
}