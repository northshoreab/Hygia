using Hygia.API.Extensions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using StructureMap;

namespace Hygia.API.Infrastructure
{
    public class ApiRequestHandler : DelegatingHandler
    {
        private readonly IContainer _container;

        public ApiRequestHandler(IContainer container)
        {
            _container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiRequest = _container.GetInstance<IApiRequest>();

            apiRequest.EnvironmentId = request.GetEnvironment();            
            apiRequest.ApiKey = request.GetApiKey();

            return base.SendAsync(request, cancellationToken);
        }
    }
}