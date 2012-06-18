using System.Linq;
using System.Net.Http;

namespace Hygia.API.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetEnvironment(this HttpRequestMessage request)
        {
            if (request.Headers.Contains("environment"))
                return request.Headers.Single(x => x.Key == "environment").Value.FirstOrDefault();

            if (request.Properties.ContainsKey("environment"))
                return request.Properties["environment"] as string;

            //var cookies = request.Headers.GetCookies().SelectMany(x => x.Cookies);
            //var cookie = cookies.SingleOrDefault(x => x.Name == "environment");

            //return cookie != null ? cookie.Value : null;
            return null;
        }

        public static string GetApiKey(this HttpRequestMessage request)
        {
            if (!request.Headers.Contains("apikey"))
                return "";

            return request.Headers.Single(x => x.Key == "apikey").Value.FirstOrDefault();
        }
    }
}