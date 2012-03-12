namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    public interface IApiCall
    {
        string Invoke(string method, string action, object message = null);
    }
}