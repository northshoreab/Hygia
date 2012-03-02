namespace Hygia.Operations.Communication.Api
{
    public class ContextInputModel
    {
        public System.Collections.Specialized.NameValueCollection Headers { get; set; }
        public System.Collections.Specialized.NameValueCollection Params { get; set; }
        public string Url { get; set; }
    }
}