namespace Hygia.API
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Operations.Communication.Domain;
    using Raven.Client;
    using StructureMap;

    public class CommandsToPickUpHandler : DelegatingHandler
    {
        private readonly IContainer container;

        public CommandsToPickUpHandler(IContainer container)
        {
            this.container = container;
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
                apiKey = request.Headers.SingleOrDefault(x => x.Key == "apikey").Value.FirstOrDefault();

            //for now assume that an api call always means that the request is coming from a launchpad
            if (apiKey != null && request.RequestUri.AbsolutePath.ToLower().StartsWith("/api/commands/"))
            {
                var session = container.GetInstance<IDocumentSession>();

                if (session.Query<LaunchPadCommand>().Any(c => !c.Delivered))
                    return true;
            }

            return false;
        }
    }
}