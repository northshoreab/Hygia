using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.Saga;

namespace Hygia.Notifications.Summary
{
    public class AlerterSummarySagaData : ISagaEntity
    {
        public Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }

        [Unique]
        public Guid AlerterInstanceId { get; set; }

        public bool IsTimeoutAlreadyRequested { get; set; }

        public Dictionary<string, DateTime> ErrorsToBeCleared;
        public Dictionary<string, FaultAlertInfo> ErrorsToAlert;

        public AlerterSummarySagaData()
        {
            ErrorsToAlert = new Dictionary<string, FaultAlertInfo>();
            ErrorsToBeCleared = new Dictionary<string, DateTime>();
        }

        public List<FaultAlertInfo> ErrorListToAlert
        {
            get { return ErrorsToAlert.Values.ToList(); }
        }

        public void ClearAlertForMessage(string originalMessageId, DateTime timeReceived)
        {
            // If its not in either list, just add to list, the message will be sent back to the queue.

            // Not in the to be alerted list, but in the cleared list
            if (!ErrorsToAlert.ContainsKey(originalMessageId) && ErrorsToBeCleared.ContainsKey(originalMessageId))
            {
                if (timeReceived > ErrorsToBeCleared[originalMessageId])
                {
                    // Update the list
                    ErrorsToBeCleared[originalMessageId] = timeReceived;
                    //WriteInfo(string.Format("ClearAlertForMessage: Updating {0} in cleared dictionary", originalMessageId));

                    return;
                }
            }

            // if the error is in the alert list and not in the cleared list
            if (ErrorsToAlert.ContainsKey(originalMessageId) && !ErrorsToBeCleared.ContainsKey(originalMessageId))
            {
                if (timeReceived > ErrorsToAlert[originalMessageId].FaultMessage.TimeOfFailure)
                {
                    //WriteInfo(string.Format("ClearAlertForMessage: Removing {0} from alert dictionary", originalMessageId));
                    ErrorsToAlert.Remove(originalMessageId);
                    return;
                }
            }

            //WriteInfo(string.Format("ClearAlertForMessage: *** {0} IN BOTH DICTIONARIES ***", originalMessageId));


            // This is in both lists! Is this even possible????!!
            if (timeReceived > ErrorsToBeCleared[originalMessageId])
            {
                ErrorsToBeCleared[originalMessageId] = timeReceived;
            }
            if (timeReceived > ErrorsToAlert[originalMessageId].FaultMessage.TimeOfFailure)
            {
                ErrorsToAlert.Remove(originalMessageId);
            }
        }

        public void AddAlertForMessage(FaultAlertInfo faultAlertInfo)
        {
            var originalMessageId = faultAlertInfo.FaultMessage.MessageId.ToString();
            // If its not in either list, just add to list
            if (!ErrorsToAlert.ContainsKey(originalMessageId) && !ErrorsToBeCleared.ContainsKey(originalMessageId))
            {
                //WriteInfo(string.Format("AddAlertForMessage: Error not in dictionary. Adding {0} to alert dictionary", originalMessageId));
                ErrorsToAlert.Add(originalMessageId, faultAlertInfo);
                return;
            }

            // Not in the to be alerted list, but in the cleared list
            if (!ErrorsToAlert.ContainsKey(originalMessageId) && ErrorsToBeCleared.ContainsKey(originalMessageId))
            {
                if (faultAlertInfo.FaultMessage.TimeOfFailure > ErrorsToBeCleared[originalMessageId])
                {
                    // Remove from the cleared list, add to the to be alerted list
                    ErrorsToBeCleared.Remove(originalMessageId);
                    ErrorsToAlert.Add(originalMessageId, faultAlertInfo);
                    //WriteInfo(string.Format("AddAlertForMessage: Removing {0} to from cleared dictionary and adding it to the Alert dictionary", originalMessageId));

                    return;
                }
            }

            // if the error is in the alert list and not in the cleared list
            if (ErrorsToAlert.ContainsKey(originalMessageId) && !ErrorsToBeCleared.ContainsKey(originalMessageId))
            {
                if (faultAlertInfo.FaultMessage.TimeOfFailure > ErrorsToAlert[originalMessageId].FaultMessage.TimeOfFailure)
                {
                    ErrorsToAlert[originalMessageId] = faultAlertInfo;
                    //WriteInfo(string.Format("AddAlertForMessage: Updating {0} in alert dictionary", originalMessageId));

                    return;
                }
            }

            //WriteInfo(string.Format("AddAlertForMessage: *** {0} IN BOTH DICTIONARIES ***", originalMessageId));

            // This is in both lists! Is this even possible????!!
            if (faultAlertInfo.FaultMessage.TimeOfFailure > ErrorsToBeCleared[originalMessageId])
            {
                ErrorsToBeCleared.Remove(originalMessageId);
            }
            if (faultAlertInfo.FaultMessage.TimeOfFailure > ErrorsToAlert[originalMessageId].FaultMessage.TimeOfFailure)
            {
                ErrorsToAlert[originalMessageId] = faultAlertInfo;
            }
        }

        public bool IsErrorInAlertList(string id)
        {
            return ErrorsToAlert.ContainsKey(id);
        }

        public bool IsErrorInToBeClearedList(string id)
        {
            return ErrorsToBeCleared.ContainsKey(id);
        }

        // Not exactly sure, if this will be useful or not. Does it make sense to hold on to how many times a particular error
        // was alerted before taking action?
        public void IncrementAlertCount()
        {
            foreach (FaultAlertInfo info in ErrorsToAlert.Values)
            {
                info.NumberOfTimesAlerted++;
            }
        }

        //public void WriteInfo(string message)
        //{
        //    string logMsg = string.Format("SagaId: {0}, ThreadName: {1}, DateTime: {2}, Message: {3}", this.Id, System.Threading.Thread.CurrentThread.Name, DateTime.Now, message);
        //    Console.WriteLine(logMsg);
        //}
    }
}