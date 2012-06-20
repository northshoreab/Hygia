using System;
using System.Collections.Generic;

namespace Hygia.Operations.Accounts.Domain
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<System> Systems { get { return _systems ?? (_systems = new List<System>()); } set { _systems = value; } }
        IList<System> _systems;
    }
}