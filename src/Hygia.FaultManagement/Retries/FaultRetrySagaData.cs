namespace Hygia.FaultManagement.Retries
{
    using System;
    using NServiceBus.Saga;

    public class FaultRetrySagaData:IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        [Unique]
        public Guid FaultId { get; set; }

        public bool RetriedSuccessFully { get; set; }
    }
}