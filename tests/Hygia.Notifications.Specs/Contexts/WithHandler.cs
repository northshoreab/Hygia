using System.Collections.Generic;
using FakeItEasy;
using System;
using Hygia.FaultManagement;
using Hygia.Notifications.Provide;
using Machine.Specifications;
using NServiceBus;
using Raven.Client;
using Raven.Client.Embedded;

namespace Hygia.Notifications.Specs.Contexts
{
    public class WithHandler<T>
    {
        protected static dynamic Handler;
        protected static IBus FakeBus;
        static IDocumentStore store;

        protected static IDocumentSession Session;

        Establish context = () =>
                                {
                                    FakeBus = A.Fake<IBus>();
                                    store = new EmbeddableDocumentStore();
                                    store.Initialize();
                                    Session = store.OpenSession();

                                    if (typeof(T) == typeof(FaultNotificationHandler))
                                        Handler = Activator.CreateInstance(typeof (T),
                                                                           new List<IProvideFaultInformation>
                                                                               {
                                                                                   new FaultInfoProvider{Session = Session},
                                                                                   new LogicalMonitoring.Handlers.FaultInfoProvider{Session = Session}
                                                                               });
                                    else
                                        Handler = Activator.CreateInstance(typeof(T));

                                    typeof(T).GetProperty("Session").SetValue(Handler, Session, null);
                                    typeof(T).GetProperty("Bus").SetValue(Handler, FakeBus, null);
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