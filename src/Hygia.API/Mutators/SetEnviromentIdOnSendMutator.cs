namespace Hygia.API.Mutators
{
    using System.Web;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;
    using StructureMap.Configuration.DSL;

    public class SetEnviromentIdOnSendMutator : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            //hack: we can't use the IFubuRequest because for some reson the context is always null.
            //this probably has to do with the way that NSB invokes the mutators. So we have to use the raw httpcontext for now
           var environment = HttpContext.Current.Request.Params["environment"];

           if (environment != null)
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