namespace Hygia.API.Infrastructure
{
    public interface IApiRequest
    {
        string EnvironmentId { get; set; }
        string ApiKey { get; set; }
    }
}