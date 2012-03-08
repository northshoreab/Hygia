namespace Hygia.Backend
{
    using NServiceBus;
    using NServiceBus.Hosting.Profiles;
    using Operations;
    using StructureMap;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .HygiaMessageConventions()
                //shows multi tennant operations of the sagas
                .MessageToDatabaseMappingConvention(context =>
                {
                    string environmentId = string.Empty;

                    if (context.Headers.ContainsKey("EnvironmentId"))
                        environmentId = context.Headers["EnvironmentId"];

                    return RavenSession.EnvironmentToDatabaseLookup(environmentId);
                })
                .StructureMapBuilder()
                .XmlSerializer()
                .RavenSubscriptionStorage()
                .RavenSagaPersister()
                .RunTimeoutManager();

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

