namespace Hygia.API.Authentication
{
    public static class Constants
    {
        public const string GithubLoginKey = "18c137f1-af1a-4ec0-8f26-2bb808de7f35";
        public const string GithubScheme = "github";
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