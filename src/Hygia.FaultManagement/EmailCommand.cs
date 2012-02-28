namespace Hygia.FaultManagement
{
    using System.Collections.Generic;

    public class EmailCommand
    {
        public EmailCommand()
        {
        }

        public EmailCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public Dictionary<string, string> Values { get; set; }
    }
}