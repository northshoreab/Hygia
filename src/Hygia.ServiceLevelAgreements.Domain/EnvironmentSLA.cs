namespace Hygia.ServiceLevelAgreements.Domain
{
    using System;

    public class EnvironmentSLA
    {
        public string Id { get; set; }
        public TimeSpan DefaultCriticalTimeSLA { get; set; }
        public TimeSpan DefaultReactionTime { get; set; }
    }
}