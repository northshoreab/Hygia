namespace Hygia.FaultManagement.LaunchPad
{
    using System;
    using System.Configuration;
    using System.Messaging;
    using System.Threading;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Faults;
    using NServiceBus.Utils;

    public class ErrorManager : INeedInitialization
    {
        const string NonTransactionalQueueErrorMessageFormat = "Queue '{0}' must be transactional.";

        const string NoMessageFoundErrorFormat =
            "INFO: No message found with ID '{0}'. Going to check headers of all messages for one with that original ID.";

        MessageQueue queue;
        static readonly TimeSpan TimeoutDuration = TimeSpan.FromSeconds(5);

        public virtual Address InputQueue
        {
            set
            {
                var path = MsmqUtilities.GetFullPath(value);
                var q = new MessageQueue(path);

                if (!q.Transactional)
                    throw new ArgumentException(string.Format(NonTransactionalQueueErrorMessageFormat, q.Path));

                queue = q;

                var mpf = new MessagePropertyFilter();
                mpf.SetAll();

                queue.MessageReadPropertyFilter = mpf;
            }
        }


        /// <summary>
        /// May throw a timeout exception if a message with the given id cannot be found.
        /// </summary>
        /// <param name="messageId"></param>
        public void ReturnMessageToSourceQueue(string messageId)
        {
            using (var scope = new TransactionScope())
            {
                var message = queue.ReceiveById(messageId, TimeoutDuration, MessageQueueTransactionType.Automatic);

                var tm = MsmqUtilities.Convert(message);

                if (!tm.Headers.ContainsKey(FaultsHeaderKeys.FailedQ))
                {
                    Console.WriteLine(
                        "ERROR: Message does not have a header indicating from which queue it came. Cannot be automatically returned to queue.");
                    return;
                }

                using (
                    var q =
                        new MessageQueue(MsmqUtilities.GetFullPath(Address.Parse(tm.Headers[FaultsHeaderKeys.FailedQ])))
                    )
                    q.Send(message, MessageQueueTransactionType.Automatic);

                Console.WriteLine("Success.");
                scope.Complete();
            }
        }

        public void Init()
        {   
            var error = ConfigurationManager.AppSettings["watchr.errors.input"];
            
            if (string.IsNullOrEmpty(error))
                return;
            
            var errorLog = ConfigurationManager.AppSettings["watchr.errors.log"];
            if (string.IsNullOrEmpty(errorLog))
                errorLog = error + "_log";

            var errorLogAddress = Address.Parse(errorLog);
            MsmqUtilities.CreateQueueIfNecessary(errorLogAddress, Thread.CurrentPrincipal.Identity.Name);

            Configure.Instance.Configurer.RegisterSingleton<ErrorManager>(new ErrorManager
                                                                              {
                                                                                  InputQueue = errorLogAddress
                                                                              });
        }
    }
}


//public void ReturnAll()
//     {
//         foreach (var m in queue.GetAllMessages())
//             ReturnMessageToSourceQueue(m.Id);
//     }

//catch (MessageQueueException ex)
//{
//    if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
//    {
//        Console.WriteLine(NoMessageFoundErrorFormat, messageId);

//        foreach (var m in queue.GetAllMessages())
//        {
//            var tm = MsmqUtilities.Convert(m);

//            if (tm.Headers.ContainsKey(TransportHeaderKeys.OriginalId))
//            {
//                if (messageId != tm.Headers[TransportHeaderKeys.OriginalId])
//                    continue;

//                Console.WriteLine("Found message - going to return to queue.");

//                using (var tx = new TransactionScope(TransactionScopeOption.RequiresNew))
//                {
//                    using (var q = new MessageQueue(
//                        MsmqUtilities.GetFullPath(
//                            Address.Parse(tm.Headers[FaultsHeaderKeys.FailedQ]))))
//                        q.Send(m, MessageQueueTransactionType.Automatic);

//                    queue.ReceiveByLookupId(MessageLookupAction.Current, m.LookupId,
//                                            MessageQueueTransactionType.Automatic);

//                    tx.Complete();
//                }

//                Console.WriteLine("Success.");
//                scope.Complete();

//                return;
//            }
//        }
//    }
//}
//            }
//        }
//    }
//}