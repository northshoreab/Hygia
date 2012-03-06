namespace Hygia.ServiceLevelAgreements.Domain
{
    using System;

    public class MessageTypeSLA
    {
        public Guid Id { get; set; }
        public TimeSpan CriticalTimeSLA { get; set; }
      
    }
}