namespace Hygia.API
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using StructureMap;

    public class ApiRequestHandler : DelegatingHandler
    {
        private readonly IContainer container;

        public ApiRequestHandler(IContainer container)
        {
            this.container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiRequest = container.GetInstance<IApiRequest>();

            apiRequest.EnvironmentId = request.GetEnvironment();            
            apiRequest.ApiKey = request.GetApiKey();

            return base.SendAsync(request, cancellationToken);
        }
    }
}