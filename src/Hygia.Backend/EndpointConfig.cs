namespace Hygia.Backend
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .HygiaMessageConventions()
                .StructureMapBuilder()
                .XmlSerializer()
                .RavenSubscriptionStorage();
        }
    }
}