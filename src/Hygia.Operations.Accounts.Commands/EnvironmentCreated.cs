using System;

namespace Hygia.Operations.Accounts.Commands
{
    public class EnvironmentCreated
    {
        public Guid Account { get; set; }
        public Guid System { get; set; }
        public Guid Environment { get; set; }
    }
}