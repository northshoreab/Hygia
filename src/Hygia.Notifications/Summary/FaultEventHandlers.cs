using System;
using System.Configuration;
using Hygia.Alarms.Events;
using NServiceBus;

namespace Hygia.Notifications.Summary
{
    class FaultEventHandlers : IHandleMessages<FaultAlarm>
    {
        public IBus Bus { get; set; }
        private readonly Guid _alerterGuid;

        public FaultEventHandlers()
        {
            Guid.TryParse(ConfigurationManager.AppSettings["AlerterInstanceId"], out _alerterGuid);
        }

        public void Handle(FaultAlarm message)
        {
            Bus.SendLocal<ProcessFaultMessageReceived>(m => { m.AlerterInstanceId = _alerterGuid; m.MessageDetails = message; });
        }
    }
}