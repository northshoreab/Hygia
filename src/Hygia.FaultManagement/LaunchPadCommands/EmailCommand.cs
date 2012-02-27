using System.Collections.Generic;

namespace Hygia.FaultManagement.LaunchPadCommands
{
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