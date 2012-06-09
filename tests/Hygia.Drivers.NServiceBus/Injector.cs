namespace Hygia.Drivers.NServiceBus
{
    using System;
    using System.Messaging;
    using System.Transactions;

    public class Injector
    {
        public string Inject(NServiceBusMessage message, string address)
        {
            var queuePath = GetFullPath(address);

            using (var q = new MessageQueue(queuePath, QueueAccessMode.Send))
            {
                var toSend = message.ToMsmqMessage();

                toSend.UseDeadLetterQueue = true;
                toSend.UseJournalQueue = true;

                q.Send(toSend, GetTransactionTypeForSend());

                return toSend.Id;
            }

        }

        public static string GetFullPath(string value)
        {
            return PREFIX + GetFullPathWithoutPrefix(value);
        }

        public static string GetFullPathWithoutPrefix(string value)
        {
            return getMachineNameFromLogicalName(value) + PRIVATE + getQueueNameFromLogicalName(value);
        }

        static string getQueueNameFromLogicalName(string logicalName)
        {
            string[] arr = logicalName.Split('@');

            if (arr.Length >= 1)
                return arr[0];

            return null;
        }
        const string PRIVATE = "\\private$\\";

        static readonly string PREFIX = "FormatName:" + DIRECTPREFIX;
        const string DIRECTPREFIX = "DIRECT=OS:";
        

        static string getMachineNameFromLogicalName(string logicalName)
        {
            string[] arr = logicalName.Split('@');

            string machine = Environment.MachineName;

            if (arr.Length >= 2)
                if (arr[1] != "." && arr[1].ToLower() != "localhost")
                    machine = arr[1];

            return machine;
        }
        static MessageQueueTransactionType GetTransactionTypeForSend()
        {
            return Transaction.Current != null ? MessageQueueTransactionType.Automatic : MessageQueueTransactionType.Single;
        }

    }
}