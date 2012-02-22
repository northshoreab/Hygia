using System;
using System.Configuration;
using System.Linq;
using NServiceBus.Saga;

namespace Hygia.Notifications.Summary
{
    class AlerterSummarySaga : Saga<AlerterSummarySagaData>,
        IAmStartedByMessages<ProcessFaultMessageReceived>
    {
        private readonly int _timeToWaitBeforeAlerting = int.Parse(ConfigurationManager.AppSettings["TimeToWaitBeforeAlerting"]);
        private readonly int _criticalErrorLimit = int.Parse(ConfigurationManager.AppSettings["CriticalErrorLimit"]);

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<ProcessFaultMessageReceived>(s => s.AlerterInstanceId, m => m.AlerterInstanceId);
        }

        public void Handle(ProcessFaultMessageReceived message)
        {
            //Data.WriteInfo(string.Format("ProcessFaultMessageReceived arrived. MessageId: {0}", message.MessageDetails.OriginalMessageId));
            Data.AlerterInstanceId = message.AlerterInstanceId;
            if (!Data.IsTimeoutAlreadyRequested)
            {
                //Data.WriteInfo(string.Format("Timeout is now requested for this saga: {0}", Data.Id));
                Data.IsTimeoutAlreadyRequested = true;
                // This is the first message received, so request a timeout. We are going to keep
                // adding all of the error messages received in the timespan and send out one alert.
                RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "summary");
            }
            else
            {
                //Data.WriteInfo(string.Format("Timeout has already been requested for this saga: {0}", Data.Id));
            }

            // Add to the list
            var alertInfo = new FaultAlertInfo { FaultMessage = message.MessageDetails, NumberOfTimesAlerted = 0 };
            Data.AddAlertForMessage(alertInfo);
        }

        public override void Timeout(object state)
        {
            //Data.WriteInfo(string.Format("Timeout has been RECEIVED for this saga: {0}", Data.Id));

            base.Timeout(state);

            // is the error queue flooded?
            if (Data.ErrorListToAlert.Count >= _criticalErrorLimit)
            {
                Bus.SendLocal<AlertTooManyErrorsInQueue>(m =>
                                                             {
                                                                 m.Count = Data.ErrorListToAlert.Count;
                                                                 m.FirstFaultMessage =
                                                                     Data.ErrorListToAlert.First().FaultMessage;
                                                             });
                Data.IncrementAlertCount();

                // Request another timeout
                RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "state");
                return;
            }

            if (Data.ErrorListToAlert.Count > 0)
            {
                var errorList = (from msg in Data.ErrorListToAlert select msg.FaultMessage).ToList();
                Bus.SendLocal<SendFaultAlert>(m =>
                {
                    m.FaultList = errorList;
                });
                Data.IncrementAlertCount();
                RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "state");
            }
            else
            {
                Data.IsTimeoutAlreadyRequested = false;
                //Data.WriteInfo("Timeout is now being cleared");
            }
        }
    }
}