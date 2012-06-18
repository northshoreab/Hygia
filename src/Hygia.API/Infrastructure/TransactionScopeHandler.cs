using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Hygia.API.Infrastructure
{
    internal class TransactionScopeHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using(var scope = new TransactionScope())
            {
                var response = base.SendAsync(request, cancellationToken);
                
                if(response.Result.IsSuccessStatusCode)
                    scope.Complete();

                return response;
            }
        }

    }
}