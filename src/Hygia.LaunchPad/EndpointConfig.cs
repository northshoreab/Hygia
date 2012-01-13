namespace Hygia.LaunchPad
{
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Document;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefaultBuilder()
                .XmlSerializer();
        }
    }

    class BootstrapRaven : IWantCustomInitialization
    {
        public void Init()
        {
            var store = new DocumentStore
                            {
                                Url = "http://localhost:8080",
                                DefaultDatabase =Configure.EndpointName

                            };

            store.Initialize();

            Configure.Instance.Configurer.RegisterSingleton<IDocumentStore>(store);
        }
    }
}