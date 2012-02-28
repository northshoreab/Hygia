namespace Hygia.Operations.Communication.Handlers
{
    using Communication;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class LaunchPadCommandHandler : IHandleMessages<ILaunchPadCommand>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(ILaunchPadCommand message)
        {
            Session.Store(new LaunchPadCommand
                              {
                                  Delivered = false,
                                  Command = message
                              });
        }
    }
}