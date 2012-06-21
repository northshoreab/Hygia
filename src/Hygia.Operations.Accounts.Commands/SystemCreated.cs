using System;

namespace Hygia.Operations.Accounts.Commands
{
    public class SystemCreated
    {
        public Guid UserAccountId { get; set; }
        public Guid SystemId { get; set; }
    }
}