using System;

namespace Hygia.Notifications.Summary.Commands
{
    public class ProcessFaultMessageReceived
    {
        public Guid AlerterInstanceId { get; set; }
        public Guid FaultId { get; set; }
    }
}