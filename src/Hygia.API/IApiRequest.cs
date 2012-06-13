namespace Hygia.API
{
    public interface IApiRequest
    {
        string EnvironmentId { get; set; }
        string ApiKey { get; set; }
    }
}