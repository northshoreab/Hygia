namespace Hygia.Web.Controllers.Operations.Accounts
{
    using System;
    using System.Collections.Generic;

    public class Account
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Environment> Environments { get { return _environments ?? (_environments = new List<Environment>()); } set { _environments = value; } }
        ICollection<Environment> _environments;
    }

    public class Environment
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ApiKey { get; set; }
    }
}