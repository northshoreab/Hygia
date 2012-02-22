using System.Collections.Generic;
using Hygia.Alarms.Events;
using NServiceBus;

namespace Hygia.Notifications.Summary
{
    public class SendFaultAlert : ICommand
    {
        public List<FaultAlarm> FaultList { get; set; }
    }
}