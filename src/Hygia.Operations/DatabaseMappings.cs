namespace Hygia.Operations
{
    using System;
    using System.Collections.Generic;

    public class DatabaseMappings
    {
        public string Id { get; set; }
        public IDictionary<Guid, string> Mappings { get; set; }
    }
}