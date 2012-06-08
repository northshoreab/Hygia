using System;

namespace Hygia.API.Models.Operations.Accounts
{
    public class Environment
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ApiKey { get; set; }
    }
}