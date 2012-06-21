using System;

namespace Hygia.Operations.Accounts.Commands
{
    public class EnvironmentCreated
    {
        public Guid SystemId { get; set; }
        public Guid EnvironmentId { get; set; }
    }
}