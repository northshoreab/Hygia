using System;

namespace Hygia.API.Models.Operations.LaunchPad
{
    public class LaunchPadStatus
    {
        public Guid Id { get; set; }
        public Guid EnvironmentId { get; set; }
        public DateTime TimeOfLastHeartBeat { get; set; }
        public string Version { get; set; }
    }
}