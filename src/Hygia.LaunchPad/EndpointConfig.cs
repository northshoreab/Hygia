namespace WatchR.LaunchPad
{
    using Hygia;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .HygiaMessageConventions()
                .DefaultBuilder()
                .XmlSerializer()
                .UseInMemoryTimeoutPersister()
                .InMemorySagaPersister();
        }
    }

}