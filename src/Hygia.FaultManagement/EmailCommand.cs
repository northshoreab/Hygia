namespace Hygia.FaultManagement
{
    using System.Collections.Generic;

    public class EmailCommand
    {
        public EmailCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public IDictionary<string, string> Values { get; set; }
    }
}