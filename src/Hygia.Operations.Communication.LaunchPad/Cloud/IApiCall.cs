namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using RestSharp;

    public interface IApiCall
    {
        string Invoke(Method method, string action, object message = null);
        RestResponse<T> Invoke<T>(Method method, string action, object message = null) where T : new();
    }
}