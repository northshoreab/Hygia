namespace Hygia.API
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Raven.Client;
    using StructureMap;

    public class RavenSessionHandlerHandler : DelegatingHandler
    {
        readonly IContainer container;

        public RavenSessionHandlerHandler(IContainer container)
        {
            this.container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.SendAsync(request, cancellationToken);

            if (response.Result.IsSuccessStatusCode)
                container.GetInstance<IDocumentSession>().SaveChanges();

            return response;
        }
    }
}