using System;

namespace Hygia.Notifications.Summary.Domain
{
    public class AlertInfo
    {
        public Guid Id { get; set; }
        public int NumberOfTimesAlerted { get; set; }
        public DateTime TimeOfAlert { get; set; }
        public object AlertMessage { get; set; }
    }
}