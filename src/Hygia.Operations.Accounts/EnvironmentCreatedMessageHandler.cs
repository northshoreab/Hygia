using Hygia.Operations.Accounts.Commands;
using NServiceBus;
using Raven.Client;
using Raven.Client.Extensions;

namespace Hygia.Operations.Accounts
{
    public class EnvironmentCreatedMessageHandler : IHandleMessages<EnvironmentCreated>
    {
        private readonly IDocumentStore store;

        public EnvironmentCreatedMessageHandler(IDocumentStore store)
        {
            this.store = store;
        }

        public void Handle(EnvironmentCreated message)
        {
            store.DatabaseCommands.EnsureDatabaseExists(message.Environment.ToString());
        }
    }
}
