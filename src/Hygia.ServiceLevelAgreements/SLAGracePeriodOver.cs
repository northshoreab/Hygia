namespace Hygia.ServiceLevelAgreements
{
    using System;
    using NServiceBus.Saga;

    public class SLAGracePeriodOver : ITimeoutState
    {
        public DateTime TimeOfBreach { get; set; }
    }
}