namespace WatchR.LaunchPad
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefaultBuilder()
                .XmlSerializer()
                .UseInMemoryTimeoutPersister()
                .InMemorySagaPersister();
        }
    }

}