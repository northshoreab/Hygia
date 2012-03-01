namespace Hygia.IntegrationTests
{
    using System;
    using Drivers.NServiceBus;
    using Machine.Specifications;
    using Raven.Client;
    using Raven.Client.Document;
    using RestSharp;

    public class BackendContext
    {
        protected static IDocumentStore Store;
        protected static Injector injector = new Injector();

        protected static Guid apiKey =  Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801");
        protected static NServiceBusMessage message;
        
        Establish context = () =>
                                {
                                    Store = new DocumentStore
                                                {
                                                    Url = "http://localhost:8080",
                                                    DefaultDatabase = "Hygia.Acme"
                                                };
                                    Store.Initialize();
                                };

        protected static void InjectMessage()
        {
            injector.Inject(message, "Hygia.Backend");
        }

    }

    public class LaunchPadContext
    {
        protected static Injector injector = new Injector();

        protected static NServiceBusMessage message;

       
        protected static void InjectMessage()
        {
            injector.Inject(message, "WatchR.LaunchPad");
        }

    }
}