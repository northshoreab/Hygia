using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers
{
    public abstract class WatchRApiController : System.Web.Http.ApiController
    {
        public IDocumentSession Session { get; set; }
        public IBus Bus { get; set; }
        public UserContext CurrentUser { get; set; }
    }
}