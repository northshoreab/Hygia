namespace Hygia.API.Infrastructure
{
    public class ApiRequest : IApiRequest
    {
        public string EnvironmentId { get; set; }
        public string ApiKey { get; set; }
    }
}