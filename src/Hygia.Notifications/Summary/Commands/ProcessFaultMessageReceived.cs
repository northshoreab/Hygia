using System;
using Hygia.Alarms.Events;
using NServiceBus;

namespace Hygia.Notifications.Summary.Commands
{
    public class ProcessFaultMessageReceived : ICommand
    {
        public Guid AlerterInstanceId { get; set; }
        public FaultAlarm MessageDetails { get; set; }
    }
}