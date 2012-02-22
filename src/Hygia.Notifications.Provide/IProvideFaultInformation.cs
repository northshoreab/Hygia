namespace Hygia.Notifications.Provide
{
    using System;
    using System.Collections.Generic;

    public interface IProvideFaultInformation:IProvide
    {
        dynamic ProvideFor(Guid faultEnvelopeId, IEnumerable<Guid> messageTypes);
    }
}