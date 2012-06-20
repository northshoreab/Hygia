using System;

namespace Hygia.Operations.Accounts.Domain
{
    public class Environment
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ApiKey { get; set; }
    }
}
