namespace Hygia.Specs
{
    using FakeItEasy;
    using System;
    using Machine.Specifications;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Embedded;

    public class WithHandler<T>
    {
        protected static dynamic Handler;
        protected static IBus FakeBus;
        static IInvokeProviders FakeProviders;
        static IDocumentStore store;

        protected static IDocumentSession Session;

        protected static dynamic ResultOfProvide = new{};

        Establish context = () =>
                                {
                                    FakeBus = A.Fake<IBus>();
                                    FakeProviders = new FakeProviderInvoker(() => DynamicHelpers.ToDynamic(ResultOfProvide));
                                    
                                    store = new EmbeddableDocumentStore();
                                    store.Initialize();
                                    Session = store.OpenSession();

                                    Handler = Activator.CreateInstance(typeof(T));

                                    typeof(T).GetProperty("Session").SetValue(Handler, Session, null);
                                    typeof(T).GetProperty("Bus").SetValue(Handler, FakeBus, null);
                                    typeof(T).GetProperty("Providers").SetValue(Handler, FakeProviders, null);
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

    internal class FakeProviderInvoker : IInvokeProviders
    {
        readonly Func<dynamic> func;

        public FakeProviderInvoker(Func<dynamic> func)
        {
            this.func = func;
        }

        public dynamic Invoke<T>(dynamic parameters) where T : IProvide
        {
            return func();
        }
    }
}