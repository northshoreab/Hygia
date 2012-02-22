using System;

namespace Hygia.ServiceLevelAgreements.Events
{
    public interface SLABreachMessage
    {
        Guid SLAId { get; set; }
        DateTime TimeOfSLABreach { get; set; }
    }
}