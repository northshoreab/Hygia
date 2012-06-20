using System;

namespace Hygia.Operations.Accounts.Commands
{
    public class SystemCreated
    {
        public Guid AccountId { get; set; }
        public Guid SystemId { get; set; }
    }
}