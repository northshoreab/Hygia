namespace Hygia.API
{
    public class ApiRequest : IApiRequest
    {
        public string EnvironmentId { get; set; }
        public string ApiKey { get; set; }
    }
}