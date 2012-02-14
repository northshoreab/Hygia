namespace Hygia.Web.Controllers
{
    using NServiceBus;
    using Raven.Client;

    public class WebController
    {
        public IDocumentSession Session { get; set; }
        public IBus Bus { get; set; }
    }
}