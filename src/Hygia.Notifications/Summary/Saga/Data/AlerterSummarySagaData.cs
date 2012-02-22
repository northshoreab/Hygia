using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Hygia.Notifications.Summary.Domain;
using NServiceBus.Saga;

namespace Hygia.Notifications.Summary.Saga.Data
{
    public class AlerterSummarySagaData : ISagaEntity
    {
        public Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }

        [Unique]
        public Guid AlerterInstanceId { get; set; }

        public bool IsTimeoutAlreadyRequested { get; set; }

        public Dictionary<string, DateTime> AlertsToBeCleared;
        public Dictionary<string, AlertInfo> AlertsToAlert;

        public AlerterSummarySagaData()
        {
            AlertsToAlert = new Dictionary<string, AlertInfo>();
            AlertsToBeCleared = new Dictionary<string, DateTime>();
        }

        public List<AlertInfo> AlertListToAlert
        {
            get { return AlertsToAlert.Values.ToList(); }
        }

        public void ClearAlertForMessage(string originalMessageId, DateTime timeReceived)
        {
            // If its not in either list, just add to list, the message will be sent back to the queue.

            // Not in the to be alerted list, but in the cleared list
            if (!AlertsToAlert.ContainsKey(originalMessageId) && AlertsToBeCleared.ContainsKey(originalMessageId))
            {
                if (timeReceived > AlertsToBeCleared[originalMessageId])
                {
                    // Update the list
                    AlertsToBeCleared[originalMessageId] = timeReceived;
                    WriteInfo(string.Format("ClearAlertForMessage: Updating {0} in cleared dictionary", originalMessageId));

                    return;
                }
            }

            // if the error is in the alert list and not in the cleared list
            if (AlertsToAlert.ContainsKey(originalMessageId) && !AlertsToBeCleared.ContainsKey(originalMessageId))
            {
                if (timeReceived > AlertsToAlert[originalMessageId].TimeOfAlert)
                {
                    WriteInfo(string.Format("ClearAlertForMessage: Removing {0} from alert dictionary", originalMessageId));
                    AlertsToAlert.Remove(originalMessageId);
                    return;
                }
            }

            WriteInfo(string.Format("ClearAlertForMessage: *** {0} IN BOTH DICTIONARIES ***", originalMessageId));

            // This is in both lists! Is this even possible????!!
            if (timeReceived > AlertsToBeCleared[originalMessageId])
            {
                AlertsToBeCleared[originalMessageId] = timeReceived;
            }
            if (timeReceived > AlertsToAlert[originalMessageId].TimeOfAlert)
            {
                AlertsToAlert.Remove(originalMessageId);
            }
        }

        public void AddAlertForMessage(AlertInfo alertInfo)
        {
            var originalMessageId = alertInfo.Id.ToString();
            // If its not in either list, just add to list
            if (!AlertsToAlert.ContainsKey(originalMessageId) && !AlertsToBeCleared.ContainsKey(originalMessageId))
            {
                WriteInfo(string.Format("AddAlertForMessage: Alert not in dictionary. Adding {0} to alert dictionary", originalMessageId));
                AlertsToAlert.Add(originalMessageId, alertInfo);
                return;
            }

            // Not in the to be alerted list, but in the cleared list
            if (!AlertsToAlert.ContainsKey(originalMessageId) && AlertsToBeCleared.ContainsKey(originalMessageId))
            {
                if (alertInfo.TimeOfAlert > AlertsToBeCleared[originalMessageId])
                {
                    // Remove from the cleared list, add to the to be alerted list
                    AlertsToBeCleared.Remove(originalMessageId);
                    AlertsToAlert.Add(originalMessageId, alertInfo);
                    WriteInfo(string.Format("AddAlertForMessage: Removing {0} to from cleared dictionary and adding it to the Alert dictionary", originalMessageId));

                    return;
                }
            }

            // if the error is in the alert list and not in the cleared list
            if (AlertsToAlert.ContainsKey(originalMessageId) && !AlertsToBeCleared.ContainsKey(originalMessageId))
            {
                if (alertInfo.TimeOfAlert > AlertsToAlert[originalMessageId].TimeOfAlert)
                {
                    AlertsToAlert[originalMessageId] = alertInfo;
                    WriteInfo(string.Format("AddAlertForMessage: Updating {0} in alert dictionary", originalMessageId));

                    return;
                }
            }

            //WriteInfo(string.Format("AddAlertForMessage: *** {0} IN BOTH DICTIONARIES ***", originalMessageId));

            // This is in both lists! Is this even possible????!!
            if (alertInfo.TimeOfAlert > AlertsToBeCleared[originalMessageId])
            {
                AlertsToBeCleared.Remove(originalMessageId);
            }
            if (alertInfo.TimeOfAlert > AlertsToAlert[originalMessageId].TimeOfAlert)
            {
                AlertsToAlert[originalMessageId] = alertInfo;
            }
        }

        public bool IsAlertInAlertList(string id)
        {
            return AlertsToAlert.ContainsKey(id);
        }

        public bool IsAlertInToBeClearedList(string id)
        {
            return AlertsToBeCleared.ContainsKey(id);
        }

        // Not exactly sure, if this will be useful or not. Does it make sense to hold on to how many times a particular alert
        // was alerted before taking action?
        public void IncrementAlertCount()
        {
            foreach (AlertInfo info in AlertsToAlert.Values)
            {
                info.NumberOfTimesAlerted++;
            }
        }

        public void WriteInfo(string message)
        {
            string logMsg = string.Format("SagaId: {0}, ThreadName: {1}, DateTime: {2}, Message: {3}", Id, Thread.CurrentThread.Name, DateTime.Now, message);
            Console.WriteLine(logMsg);
        }
    }
}