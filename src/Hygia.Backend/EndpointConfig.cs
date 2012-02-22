namespace Hygia.Backend
{
    using NServiceBus;
    using StructureMap;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            ObjectFactory.Configure(c=>
                c.Scan(s=>
                           {
                               s.LookForRegistries();
                               s.AssembliesFromApplicationBaseDirectory();
                           }));
            Configure.With()
                .HygiaMessageConventions()
                .StructureMapBuilder()
                .XmlSerializer()
                .RavenSubscriptionStorage();
        }
    }
}