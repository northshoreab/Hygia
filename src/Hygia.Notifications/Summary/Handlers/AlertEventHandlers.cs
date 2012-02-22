using System;
using System.Configuration;
using Hygia.Notifications.Summary.Commands;
using Hygia.ServiceLevelAgreements.Events;
using NServiceBus;

namespace Hygia.Notifications.Summary.Handlers
{
    using FaultManagement.Events;

    class AlertEventHandlers : IHandleMessages<FaultRegistered>,
                               IHandleMessages<SLABreachMessage>
    {
        public IBus Bus { get; set; }
        private readonly Guid _alerterInstanceId;

        public AlertEventHandlers()
        {
            Guid.TryParse(ConfigurationManager.AppSettings["AlerterInstanceId"], out _alerterInstanceId);
        }

        public void Handle(FaultRegistered message)
        {
            Bus.SendLocal<ProcessFaultMessageReceived>(m => { m.AlerterInstanceId = _alerterInstanceId; m.FaultId = message.EnvelopeId; });
        }

        public void Handle(SLABreachMessage message)
        {
            Bus.SendLocal<ProcessSLABreachMessageReceived>(m => { m.AlerterInstanceId = _alerterInstanceId; m.MessageDetails = message; });
        }
    }
}