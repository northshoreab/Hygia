using Hygia.Core;

namespace Hygia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public static class HeaderExtensions
    {
        const string ProcessingStartedHeader = "NServiceBus.ProcessingStarted";
        const string ProcessingEndedHeader = "NServiceBus.ProcessingEnded";
        const string TimeSentHeader = "NServiceBus.TimeSent";

        public static IEnumerable<string> MessageTypes(this IDictionary<string, string> headers)
        {
            var result = new List<string>();

            if (!headers.ContainsKey("NServiceBus.EnclosedMessageTypes"))
                return result;

            return
                headers["NServiceBus.EnclosedMessageTypes"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                    ToList();
        }


        public static DateTime TimeSent(this IDictionary<string, string> headers)
        {
            if (!headers.ContainsKey(TimeSentHeader))
                return DateTime.MinValue;

            return headers[TimeSentHeader].ToUtcDateTime();
        }


        public static DateTime ProcessingStarted(this IDictionary<string, string> headers)
        {
            if (!headers.ContainsKey(ProcessingStartedHeader))
                return DateTime.MinValue;

            return headers[ProcessingStartedHeader].ToUtcDateTime();
        }


        public static DateTime ProcessingEnded(this IDictionary<string, string> headers)
        {
            if (!headers.ContainsKey(ProcessingEndedHeader))
                return DateTime.MinValue;

            return headers[ProcessingEndedHeader].ToUtcDateTime();
        }

        public static TimeSpan CriticalTime(this IDictionary<string, string> headers)
        {
            var sent = headers.TimeSent();
            var ended = headers.ProcessingEnded();

            if (sent == DateTime.MinValue || ended == DateTime.MinValue || sent > ended)
                return TimeSpan.Zero;

            return ended - sent;
        }


        public static TimeSpan ProcessingTime(this IDictionary<string, string> headers)
        {
            var started = headers.ProcessingStarted();
            var ended = headers.ProcessingEnded();

            if (started == DateTime.MinValue || ended == DateTime.MinValue || started > ended)
                return TimeSpan.Zero;

            return ended - started;
        }
    }

    public static class MessageTypeExtensions
    {
        public static Guid MessageTypeId(this string messageType)
        {
            var typeName = messageType.Split(',').First();
            return typeName.ToGuid();
        }
    }
}
