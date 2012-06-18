namespace Hygia.API.Infrastructure.Authentication
{
    public class GithubLoginToken
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string LoginKey { get; set; }
    }
}