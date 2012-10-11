using System;
using System.Collections.Generic;

namespace Hygia.Operations.Accounts.Domain
{
    public class Environment
    {
        public Environment()
        {
            Users = new List<Guid>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ApiKey { get; set; }
        public IList<Guid> Users { get; set; }
    }
}
