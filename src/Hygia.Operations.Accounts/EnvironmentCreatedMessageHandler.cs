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

            //TODO: Create authorization roles needed and make sure all admins of the system has authorization to this environment
        }
    }
}
