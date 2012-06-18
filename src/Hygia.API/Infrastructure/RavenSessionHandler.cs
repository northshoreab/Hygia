using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using StructureMap;

namespace Hygia.API.Infrastructure
{
    public class RavenSessionHandler : DelegatingHandler
    {
        readonly IContainer _container;

        public RavenSessionHandler(IContainer container)
        {
            _container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.SendAsync(request, cancellationToken);

            if (response.Result.IsSuccessStatusCode && response.Status != TaskStatus.Faulted)
                _container.GetInstance<IDocumentSession>().SaveChanges();

            return response;
        }
    }
}