namespace Hygia.Backend
{
    using NServiceBus;
    using NServiceBus.Hosting.Profiles;
    using StructureMap;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .HygiaMessageConventions()
                .StructureMapBuilder()
                .XmlSerializer()
                .RavenSubscriptionStorage();

            ObjectFactory.Configure(c =>
             c.Scan(s =>
             {
                 s.LookForRegistries();
                 s.AssembliesFromApplicationBaseDirectory();
             }));
         
        }
    }

    public class ProfileHandlers:IHandleProfile<Integration>
    {
        public void ProfileActivated()
        {
            ObjectFactory.Container.SetDefaultsToProfile("Integration");
        }
    }
}

