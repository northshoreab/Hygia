namespace Hygia.LaunchPad.Specs.Contexts
{
    using System;
    using Machine.Specifications;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Embedded;

    public class WithHandler<T>
    {
        protected static dynamic Handler;
        static IBus mockBus;
        static IDocumentStore store;

        protected static IDocumentSession Session;

        Establish context = () =>
                                {
                                    store = new EmbeddableDocumentStore();
                                    store.Initialize();
                                    Session = store.OpenSession();
                                    Handler = Activator.CreateInstance(typeof(T)) as dynamic;
                                    typeof (T).GetProperty("Session").SetValue(Handler, Session, null);

                                };

        protected static void Handle<TMessage>(Action<TMessage> a) where TMessage : new()
        {
            var message = new TMessage();
            a(message);

            Handle(message);
        }
        protected static void Handle<TMessage>(TMessage message)
        {
            Handler.Handle(message);
            Session.SaveChanges();
        }


    }
}