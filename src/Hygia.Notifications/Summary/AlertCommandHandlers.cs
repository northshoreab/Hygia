using System;
using System.Collections.Generic;
using System.Linq;
using Hygia.Alarms.Events;
using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications.Summary
{
    class AlertCommandHandlers : IHandleMessages<SendFaultAlert>, IHandleMessages<AlertTooManyErrorsInQueue>
    {
        private readonly IBus _bus;
        private readonly IDocumentSession _session;

        public AlertCommandHandlers(IBus bus, IDocumentSession session)
        {
            _bus = bus;
            _session = session;
        }

        public void Handle(SendFaultAlert message)
        {
            foreach (var faultSummarySetting in _session.Query<FaultSummarySetting>())
            {
                _bus.Publish(new SendEmailCommand
                                 {
                                     Body = CreateFaultSummary(message.FaultList),
                                     Subject = "Fault summary - " + DateTime.Now,
                                     To = faultSummarySetting.Email
                                 });
            }
        }

        public void Handle(AlertTooManyErrorsInQueue message)
        {
            foreach (var faultSummarySetting in _session.Query<FaultSummarySetting>())
            {
                _bus.Publish(new SendEmailCommand
                                 {
                                     Body = "Too many errors in queue!!",
                                     Subject = "Too many errors in queue!\n\nFirst fault message: " + message.FirstFaultMessage.MessageTypeId,
                                     To = faultSummarySetting.Email
                                 });
            }
        }

        private string CreateFaultSummary(IEnumerable<FaultAlarm> faultAlarms)
        {
            string summary = "";

            foreach (var faultAlarm in faultAlarms.OrderBy(x => x.TimeOfFailure))
            {
                summary += "Time: " + faultAlarm.TimeOfFailure + " MessageTypeId: " + faultAlarm.MessageTypeId +
                           " Reason: " + faultAlarm.ExceptionReason + "\n";

            }

            return summary;
        }
    }
}