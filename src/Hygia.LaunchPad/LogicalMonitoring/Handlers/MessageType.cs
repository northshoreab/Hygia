namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System.Collections.Generic;
    using Inspectors;

    public class MessageType
    {
        ICollection<string> versions;

        public string Id { get; set; }

        public string Type { get; set; }

        public MessageIntent Intent { get; set; }

        public ICollection<string> Versions
        {
            get
            {
                return versions ?? new List<string>();
            }
            set
            {
                versions = value;
            }
        }
    }
}