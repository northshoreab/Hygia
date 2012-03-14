namespace Hygia.Specs
{
    using Machine.Specifications;
    using Raven.Client;
    using Raven.Client.Embedded;

    public class RavenContext
    {
        static IDocumentStore store;


        protected static IDocumentSession Session;



        Establish context = () =>
                                {
                                    store = new EmbeddableDocumentStore();
                                    store.Initialize();
                                    Session = store.OpenSession();
                                };



        Cleanup after_each = () =>
        {
            Session.Dispose();
            store.Dispose();
        };
    }
}