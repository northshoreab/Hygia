using System;
using System.Collections.Generic;
using System.Linq;
using Hygia.Notifications.Domain;
using Hygia.Notifications.Summary.Commands;
using Hygia.Notifications.Summary.Domain;
using Hygia.Operations.Email.Commands;
using Hygia.ServiceLevelAgreements.Events;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications.Summary.Handlers
{
    class AlertCommandHandlers : IHandleMessages<SendAlert>, IHandleMessages<AlertTooManyAlertsInQueue>
    {
        private readonly IBus _bus;
        private readonly IDocumentSession _session;

        public AlertCommandHandlers(IBus bus, IDocumentSession session)
        {
            _bus = bus;
            _session = session;
        }

        public void Handle(SendAlert message)
        {
            foreach (var alertSummarySetting in _session.Query<AlertSummarySetting>())
            {
                _bus.Publish(new SendEmailRequest
                                 {
                                     Body = CreateAlertSummary(message.AlertList),
                                     Subject = "Alert summary - " + DateTime.Now,
                                     To = alertSummarySetting.Email
                                 });
            }
        }

        public void Handle(AlertTooManyAlertsInQueue message)
        {
            foreach (var alertSummarySetting in _session.Query<AlertSummarySetting>())
            {
                _bus.Publish(new SendEmailRequest
                                 {
                                     Body = "Too many alerts in queue!!",
                                     Subject = "Too many alerts in queue!\n\nFirst alert id: " + message.FirstAlert.Id,
                                     To = alertSummarySetting.Email
                                 });
            }
        }

        private string CreateAlertSummary(IEnumerable<AlertInfo> alerts)
        {
            string summary = "";

            //foreach (var alert in alerts.Where(x => x.AlertMessage.GetType() == typeof(FaultAlarm)).OrderBy(x => x.TimeOfAlert))
            //{
            //    var message = (FaultAlarm) alert.AlertMessage;
            //    summary += "Time: " + alert.TimeOfAlert + " MessageTypeId: " + message.MessageTypeId +
            //               " Reason: " + message.ExceptionReason + "\n";

            //}

            foreach (var alert in alerts.Where(x => x.AlertMessage.GetType() == typeof(SLABreachMessage)).OrderBy(x => x.TimeOfAlert))
            {
                var message = (SLABreachMessage) alert.AlertMessage;
                summary += "Time: " + alert.TimeOfAlert + " SLAId: " + message.SLAId;
            }

            return summary;
        }
    }
}