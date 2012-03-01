namespace Hygia.Drivers.NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Messaging;
    using System.Text;
    using System.Xml.Serialization;

    public class NServiceBusMessage
    {
        readonly IList<NSBMessage> messages = new List<NSBMessage>();

        public NServiceBusMessage()
        {
            MessageIntent = MessageIntent.Send;
            Headers = new Dictionary<string, string>();
            IdForCorrelation = Guid.NewGuid().ToString();
            TimeToBeReceived = MessageQueue.InfiniteTimeout;
        }

        public string ReplyToAddress { get; set; }

        public MessageIntent MessageIntent { get; set; }

        public string CorrelationId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string IdForCorrelation { get; set; }

        public TimeSpan TimeToBeReceived { get; set; }

        public byte[] Body { get; set; }

        bool processingTimesSet;

        public NServiceBusMessage ProcessingTimes(DateTime sent, DateTime received, DateTime processed)
        {
            Headers["NServiceBus.TimeSent"] = sent.ToWireFormattedString();
            Headers["NServiceBus.ProcessingStarted"] = received.ToWireFormattedString();
            Headers["NServiceBus.ProcessingEnded"] = processed.ToWireFormattedString();
            processingTimesSet = true;
            return this;
        }

        public NServiceBusMessage SimulateProcessingTimes()
        {
            var now = DateTime.UtcNow;

            ProcessingTimes(now, now.AddSeconds(0.5), now.AddSeconds(2));

            return this;
        }

        public NServiceBusMessage AddMessage(string typeName, string version)
        {

            return this.AddMessage(typeName, version, null);
        }

        public NServiceBusMessage AddMessage(string typeName, string version, dynamic message)
        {
            messages.Add(new NSBMessage
            {
                MessageType = string.Format("{0}, Version={1}, Culture=neutral, PublicKeyToken=null", typeName, version),
                Body = message
            });

            return this;
        }

        public NServiceBusMessage AddMessage(string assemblyQualifiedName)
        {
            messages.Add(new NSBMessage
            {
                MessageType = assemblyQualifiedName,
                Body = null
            });

            return this;
        }

        public NServiceBusMessage AddMessage(Type messageType)
        {
            return this.AddMessage(messageType.AssemblyQualifiedName);
        }


        public Message ToMsmqMessage()
        {
            SerializeMessages();

            if (!processingTimesSet)
                SimulateProcessingTimes();

            var result = new Message();

            if (Body != null)
                result.BodyStream = new MemoryStream(Body);

            if (CorrelationId != null)
                result.CorrelationId = CorrelationId;

            result.Recoverable = true;

            if (TimeToBeReceived < MessageQueue.InfiniteTimeout)
                result.TimeToBeReceived = TimeToBeReceived;

            if (Headers == null)
                Headers = new Dictionary<string, string>();

            if (!Headers.ContainsKey("CorrId"))
                Headers.Add("CorrId", null);

            if (String.IsNullOrEmpty(Headers["CorrId"]))
                Headers["CorrId"] = IdForCorrelation;

            using (var stream = new MemoryStream())
            {
                headerSerializer.Serialize(stream, Headers.Select(pair => new HeaderInfo { Key = pair.Key, Value = pair.Value }).ToList());
                result.Extension = stream.GetBuffer();
            }

            result.AppSpecific = (int)MessageIntent;
            if (ReplyToAddress != null)
                result.ResponseQueue = new MessageQueue(Injector.GetFullPath(ReplyToAddress));


            return result;
        }

        void SerializeMessages()
        {
            var message = messages.First();

            //for now just add the header
            Headers[EnclosedMessageTypes] = string.Join(";", messages.Select(m=>m.MessageType));

            var rootType = message.MessageType.Split(',').First();
            var root = rootType.Split('.').Last();

            var nsTemp = message.MessageType.Split(',').First().Split('.').ToList();

            nsTemp.RemoveAt(nsTemp.Count-1);

            var ns = string.Join(".", nsTemp);

            var result = "<?xml version='1.0' ?><Messages xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://tempuri.net/" + ns + "'><" + root + ">";

            foreach (var pair in (IDictionary<string,object>)ToDynamic( message.Body) )
            {
                result += string.Format("<{0}>{1}</{0}>",pair.Key,pair.Value);
            }
            result += "</" + root + "></Messages>";

            Body = Encoding.UTF8.GetBytes(result);
        }

        public const string EnclosedMessageTypes = "NServiceBus.EnclosedMessageTypes";
        readonly XmlSerializer headerSerializer = new XmlSerializer(typeof(List<HeaderInfo>));

        public static dynamic ToDynamic(object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }

    internal class NSBMessage
    {
        public string MessageType { get; set; }
        public dynamic Body { get; set; }
    }

    public enum MessageIntent
    {
        ///<summary>
        /// Regular point-to-point send
        ///</summary>
        Send,

        ///<summary>
        /// Publish, not a regular point-to-point send
        ///</summary>
        Publish,

        /// <summary>
        /// Subscribe
        /// </summary>
        Subscribe,

        /// <summary>
        /// Unsubscribe
        /// </summary>
        Unsubscribe
    }

    [Serializable]
    public class HeaderInfo
    {
        /// <summary>
        /// The key used to lookup the value in the header collection.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value stored under the key in the header collection.
        /// </summary>
        public string Value { get; set; }
    }

    public static class DateTimeExtensions
    {
        const string Format = "yyyy-MM-dd HH:mm:ss:ffffff Z";

        /// <summary>
        /// Converts the date time to a string suitable for transport over the wire
        /// </summary>
        /// <returns></returns>
        public static string ToWireFormattedString(this DateTime time)
        {
            return time.ToUniversalTime().ToString(Format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the date time to a string suitable for transport over the wire
        /// </summary>
        /// <returns></returns>
        public static DateTime ToUtcDateTime(this string time)
        {
            return DateTime.ParseExact(time, Format, CultureInfo.InvariantCulture).ToUniversalTime();
        }
    }
}
