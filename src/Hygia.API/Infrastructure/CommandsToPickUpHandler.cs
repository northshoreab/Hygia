using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hygia.Operations.Communication.Domain;
using Raven.Client;
using StructureMap;

namespace Hygia.API.Infrastructure
{
    public class CommandsToPickUpHandler : DelegatingHandler
    {
        private readonly IContainer _container;

        public CommandsToPickUpHandler(IContainer container)
        {
            _container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
                                                                               {
                                                                                   HttpResponseMessage response = task.Result;

                                                                                   if (CommandsAvailable(request))
                                                                                       response.Headers.Add(
                                                                                           "watchr.commandsavailable",
                                                                                           "true");
                                                                                   return response;
                                                                               });

        }

        private bool CommandsAvailable(HttpRequestMessage request)
        {
            string apiKey = null;

            if (request.Headers.Contains("apikey"))
                apiKey = request.Headers.SingleOrDefault(x => x.Key.ToLower() == "apikey").Value.FirstOrDefault();

            //for now assume that an api call always means that the request is coming from a launchpad
            if (apiKey != null && request.RequestUri.AbsolutePath.ToLower().StartsWith("/api/commands/"))
            {
                var session = _container.GetInstance<IDocumentSession>();

                if (session.Query<LaunchPadCommand>().Any(c => !c.Delivered))
                    return true;
            }

            return false;
        }
    }
}