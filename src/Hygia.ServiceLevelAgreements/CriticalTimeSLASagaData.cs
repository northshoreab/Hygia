namespace Hygia.ServiceLevelAgreements
{
    using System;
    using NServiceBus.Saga;

    public class CriticalTimeSLASagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        [Unique]
        public Guid MessageTypeId { get; set; }

        public bool SLABreached { get; set; }

        public DateTime SLABreachedAt { get; set; }
    }
}