namespace Hygia.Specs
{
    using System.Linq;
    using System;
    using Machine.Specifications;

    public class WithHandler<T> : RavenContext
    {
        protected static dynamic Handler;
        protected static FakeBus FakeBus;
        static IInvokeProviders FakeProviders;
        protected static dynamic ResultOfProvide = new { };

        Establish context = () =>
                                {
                                    FakeBus = new FakeBus();
                                    FakeProviders = new FakeProviderInvoker(() => DynamicHelpers.ToDynamic(ResultOfProvide));

                                    Handler = Activator.CreateInstance(typeof(T));

                                    if (typeof(T).GetProperty("Session") != null)
                                        typeof(T).GetProperty("Session").SetValue(Handler, Session, null);
                                    if (typeof(T).GetProperty("Bus") != null)
                                        typeof(T).GetProperty("Bus").SetValue(Handler, FakeBus, null);
                                    if (typeof(T).GetProperty("Providers") != null)
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

        protected static void AssertWasSent<TMessage>(Predicate<TMessage> predicate = null)
        {
            if (predicate == null)
                predicate = message => true;

            FakeBus.SentMessages.Any(m =>
                                         {
                                             if(!(m is TMessage))
                                                 return false;
                                             var message = (TMessage) m;

                                             return predicate(message);
                                         }).ShouldBeTrue();
        }

        protected static void AssertWasNotSent<TMessage>()
        {
            FakeBus.SentMessages.Any(m => m.GetType() == typeof(TMessage)).ShouldBeFalse();
        }

    }
}