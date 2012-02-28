namespace Hygia.Operations.Communication.Handlers
{
    using Communication;
    using Raven.Client;

    public class LaunchPadCommandPersister : ILaunchPadCommand
    {
        public IDocumentSession Session { get; set; }

        public void Send(object command)
        {
            Session.Store(command);
        }
    }
}