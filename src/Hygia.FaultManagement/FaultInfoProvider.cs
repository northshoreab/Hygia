namespace Hygia.FaultManagement
{
    using Domain;
    using Notifications.Provide;
    using Raven.Client;
    
    public class FaultInfoProvider:IProvideFaultInformation
    {
        public IDocumentSession Session { get; set; }
        public dynamic ProvideFor(dynamic parameters)
        {
            var fault = Session.Load<Fault>(parameters.FaultEnvelopeId);

            return new
                       {
                           fault.Id,
                           fault.TimeOfFailure,
                           fault.Exception.Message,
                           fault.Exception.Reason,
                           fault.Body,
                           fault.Headers,
                           fault.Retries
                       };
        }
    }
}