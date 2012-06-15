namespace Hygia.API.Authentication
{
    public static class Constants
    {
        public static class ClaimTypes
        {
            public const string WatchRRole = "http://watchr.se/claims/watchrrole";
            public const string UserAccountId = "http://watchr.se/claims/useraccountid";
            public const string GithubAccessToken = "http://watchr.se/claims/githubaccesstoken";
        }

        public static class Issuers
        {
            public const string Github = "github.com";
        }
    }
}