namespace Hygia.Operations.Communications.LaunchPad
{
    using Raven.Client;

    public class LaunchPadCommandPersister : ILaunchPadCommands
    {
        public IDocumentSession Session { get; set; }

        public void Send(object command)
        {
            Session.Store(command);
        }
    }
}