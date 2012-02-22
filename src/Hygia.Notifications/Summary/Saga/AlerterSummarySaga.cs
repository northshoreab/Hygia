using System;
using System.Configuration;
using System.Linq;
using Hygia.Notifications.Summary.Commands;
using Hygia.Notifications.Summary.Domain;
using Hygia.Notifications.Summary.Saga.Data;
using NServiceBus.Saga;

namespace Hygia.Notifications.Summary.Saga
{
    //class AlerterSummarySaga : Saga<AlerterSummarySagaData>,
    //                           IAmStartedByMessages<ProcessFaultMessageReceived>,
    //                           IAmStartedByMessages<ProcessSLABreachMessageReceived>
    //{
    //    private readonly int _timeToWaitBeforeAlerting = int.Parse(ConfigurationManager.AppSettings["TimeToWaitBeforeAlerting"]);
    //    private readonly int _criticalAlertLimit = int.Parse(ConfigurationManager.AppSettings["CriticalAlertLimit"]);

    //    public override void ConfigureHowToFindSaga()
    //    {
    //        ConfigureMapping<ProcessFaultMessageReceived>(s => s.AlerterInstanceId, m => m.AlerterInstanceId);
    //    }

    //    public void Handle(ProcessFaultMessageReceived message)
    //    {
    //        Data.WriteInfo(string.Format("ProcessFaultMessageReceived arrived. MessageId: {0}", message.FaultId));
    //        Data.AlerterInstanceId = message.AlerterInstanceId;
    //        if (!Data.IsTimeoutAlreadyRequested)
    //        {
    //            Data.WriteInfo(string.Format("Timeout is now requested for this saga: {0}", Data.Id));
    //            Data.IsTimeoutAlreadyRequested = true;
    //            // This is the first message received, so request a timeout. We are going to keep
    //            // adding all of the error messages received in the timespan and send out one alert.
    //            RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "summary");
    //        }
    //        else
    //        {
    //            Data.WriteInfo(string.Format("Timeout has already been requested for this saga: {0}", Data.Id));
    //        }

    //        // Add to the list
    //        var alertInfo = new AlertInfo { TimeOfAlert = DateTime.Now, Id = message.FaultId, NumberOfTimesAlerted = 0 };
    //        Data.AddAlertForMessage(alertInfo);
    //    }

    //    public override void Timeout(object state)
    //    {
    //        Data.WriteInfo(string.Format("Timeout has been RECEIVED for this saga: {0}", Data.Id));

    //        base.Timeout(state);

    //        // is the error queue flooded?
    //        if (Data.AlertListToAlert.Count >= _criticalAlertLimit)
    //        {
    //            Bus.SendLocal<AlertTooManyAlertsInQueue>(m =>
    //                                                         {
    //                                                             m.Count = Data.AlertListToAlert.Count;
    //                                                             m.FirstAlert = Data.AlertListToAlert.First();
    //                                                         });
    //            Data.IncrementAlertCount();

    //            // Request another timeout
    //            RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "state");
    //            return;
    //        }

    //        if (Data.AlertListToAlert.Count > 0)
    //        {
    //            Bus.SendLocal<SendAlert>(m =>
    //                                              {
    //                                                  m.AlertList = Data.AlertListToAlert;
    //                                              });
    //            Data.IncrementAlertCount();
    //            RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "state");
    //        }
    //        else
    //        {
    //            Data.IsTimeoutAlreadyRequested = false;
    //            Data.WriteInfo("Timeout is now being cleared");
    //        }
    //    }

    //    public void Handle(ProcessSLABreachMessageReceived message)
    //    {
    //        Data.WriteInfo(string.Format("ProcessSLABreachMessageReceived arrived. SLAId: {0}", message.MessageDetails.SLAId));
    //        Data.AlerterInstanceId = message.AlerterInstanceId;
    //        if (!Data.IsTimeoutAlreadyRequested)
    //        {
    //            Data.WriteInfo(string.Format("Timeout is now requested for this saga: {0}", Data.Id));
    //            Data.IsTimeoutAlreadyRequested = true;
    //            // This is the first message received, so request a timeout. We are going to keep
    //            // adding all of the error messages received in the timespan and send out one alert.
    //            RequestUtcTimeout(TimeSpan.FromSeconds(_timeToWaitBeforeAlerting), "summary");
    //        }
    //        else
    //        {
    //            Data.WriteInfo(string.Format("Timeout has already been requested for this saga: {0}", Data.Id));
    //        }

    //        // Add to the list
    //        var alertInfo = new AlertInfo
    //                            {
    //                                AlertMessage = message.MessageDetails,
    //                                NumberOfTimesAlerted = 0,
    //                                Id = message.MessageDetails.SLAId,
    //                                TimeOfAlert = message.MessageDetails.TimeOfSLABreach
    //                            };

    //        Data.AddAlertForMessage(alertInfo);
    //    }
    //}
}