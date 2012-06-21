using System;
using System.Collections.Generic;

namespace Hygia.Operations.Accounts.Domain
{
    public class System
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<Environment> Environments { get { return _environments ?? (_environments = new List<Environment>()); } set { _environments = value; } }

        public Guid OwnedBy { get; set; }

        IList<Environment> _environments;
    }
}