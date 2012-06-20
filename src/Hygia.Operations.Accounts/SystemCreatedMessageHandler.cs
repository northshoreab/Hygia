using Hygia.Operations.Accounts.Commands;
using NServiceBus;

namespace Hygia.Operations.Accounts
{
    public class SystemCreatedMessageHandler : IHandleMessages<SystemCreated>
    {
        public void Handle(SystemCreated message)
        {
            //TODO: Create authorization roles needed and make sure all admins of the account has authorization to this environment
        }
    }
}