using Hygia.API.Infrastructure;

namespace Hygia.API.Mutators
{
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;
    using StructureMap.Configuration.DSL;

    public class SetEnviromentIdOnSendMutator : IMutateOutgoingTransportMessages
    {
        private readonly IApiRequest _apiRequest;

        public SetEnviromentIdOnSendMutator(IApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            //hack: we can't use the IFubuRequest because for some reson the context is always null.
            //this probably has to do with the way that NSB invokes the mutators. So we have to use the raw httpcontext for now
           //var environment = HttpContext.Current.Request.Params["environment"];

           //if(environment == null)
           //    environment = HttpContext.Current.Request.Params["apikey"];

           //if (environment == null)
           //    environment = HttpContext.Current.Request.Headers["apikey"];

            string environment = _apiRequest.EnvironmentId;

            if (!string.IsNullOrEmpty(environment))
                transportMessage.Headers["EnvironmentId"] = environment;
        }

    }

    public class SetEnviromentIdOnSendMutatorRegistry : Registry
    {
        public SetEnviromentIdOnSendMutatorRegistry()
        {
            For<IMutateOutgoingTransportMessages>().HybridHttpOrThreadLocalScoped()
                .Add<SetEnviromentIdOnSendMutator>();
        }
    }
}