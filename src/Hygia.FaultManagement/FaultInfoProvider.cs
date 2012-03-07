namespace Hygia.FaultManagement
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Notifications.Provide;
    using Raven.Client;

    public class FaultInfoProvider:IProvideFaultInformation
    {
        public IDocumentSession Session { get; set; }
        public dynamic ProvideFor(Guid faultEnvelopeId, IEnumerable<Guid> messageTypes)
        {
            var fault = Session.Load<Fault>(faultEnvelopeId);

            return new
                       {
                           fault.TimeOfFailure,
                           fault.Exception.Message,
                           fault.Exception.Reason,
                           fault.Body,
                           fault.Headers
                       };
        }
    }
}