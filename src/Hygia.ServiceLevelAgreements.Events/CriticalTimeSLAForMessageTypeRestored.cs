namespace Hygia.ServiceLevelAgreements.Events
{
    using System;

    public class CriticalTimeSLAForMessageTypeRestored
    {
        public Guid MessageTypeId { get; set; }
    }
}