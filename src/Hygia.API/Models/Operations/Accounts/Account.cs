using System;
using System.Collections.Generic;

namespace Hygia.API.Models.Operations.Accounts
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Environment> Environments { get { return _environments ?? (_environments = new List<Environment>()); } set { _environments = value; } }
        ICollection<Environment> _environments;
    }
}