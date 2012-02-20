using System;

namespace Hygia.Alarms.Events
{
    public class ErrorMessageAlarm : AlarmMessage
    {
        public Guid MessageTypeId { get; set; }
        public string Exception { get; set; }
        public string MessageBody { get; set; }
    }
}