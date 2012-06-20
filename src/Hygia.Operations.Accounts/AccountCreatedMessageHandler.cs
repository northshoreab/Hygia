using Hygia.Operations.Accounts.Commands;
using NServiceBus;

namespace Hygia.Operations.Accounts
{
    public class AccountCreatedMessageHandler : IHandleMessages<AccountCreated>
    {
        public void Handle(AccountCreated message)
        {
            //TODO: Create authorization role for account and add user to the role
        }
    }
}