namespace Hygia.API.Controllers.UserManagement.GitHub
{
//    [DefaultHttpRouteConvention]
//    [RoutePrefix("api/usermanagement/github")]
//    public class GithubController : ApiController
//    {
//        private readonly IDocumentSession _session;

//        public GithubController(IDocumentSession session)
//        {
//            _session = session;
//        }

//        public GithubSignUpInputModel GetAll()
//        {
//            return new GithubSignUpInputModel();
//        }

//        public UserAccount Post(GithubSignUpInputModel model)
//        {
//            var accessToken = GetGithubAccessToken(model.Code);

//            //get user details so that we can auto suggest repos etc
//            var userDetailsRequest = new RestRequest("/user", Method.GET) { RequestFormat = DataFormat.Json };

////            var user = User.Identity as IClaimsIdentity;

////            if(user == null)
////                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));

////            Claim accessToken = user.Claims.Single(x => x.ClaimType == "GitHubAccessToken");
//            userDetailsRequest.AddParameter("access_token", accessToken);

//            var client = new RestClient("https://api.github.com");

//            var response = client.Execute<GithubUserResponse>(userDetailsRequest);

//            if (response.StatusCode != HttpStatusCode.OK)
//                throw new Exception(response.StatusDescription);

//            var githubUserId = response.Data.id;

//            var account = _session.Query<UserAccount>().SingleOrDefault(u => u.Email == response.Data.email);


//            //see if we alrady has this user (by email)
//            if (account != null && account.GithubUserId != githubUserId)
//            {
//                if (account.GithubUserId != null)
//                    throw new InvalidOperationException("Account with the same email already exists and is associated with another Github account");

//                account.GithubUserId = response.Data.id;
//                account.Status = UserAccountStatus.Verified;

//                return account;
//            }

//            //see if we already has this user (by github id)
//            account = _session.Query<UserAccount>().SingleOrDefault(u => u.GithubUserId == githubUserId);

//            if (account != null)
//                return account;

//            var userId = response.Data.id.ToGuid();

//            account = new UserAccount
//            {
//                Id = userId,
//                UserName = response.Data.email,
//                Email = response.Data.email,
//                SignedUpAt = DateTime.UtcNow,
//                Status = UserAccountStatus.Verified,
//                GithubUserId = githubUserId,
//                GravatarId = response.Data.gravatar_id,
//                GithubAccessToken = accessToken
//            };

//            _session.Store(account);

//            return account;
//        }

        ////
        //static string GetGithubAccessToken(string code)
        //{
        //    var client = new RestClient("https://github.com");

        //    var accessTokenRequest = new RestRequest("/login/oauth/access_token", Method.POST) { RequestFormat = DataFormat.Json };

        //    accessTokenRequest.AddParameter("client_id", "933251074a0f47066f44");
        //    accessTokenRequest.AddParameter("client_secret", "11c5684e3f03e9efd49d3c7b663dcc0d36cf6bda");
        //    accessTokenRequest.AddParameter("code", code);

        //    var response = client.Execute<AccessTokenResponse>(accessTokenRequest);

        //    if (response.StatusCode != HttpStatusCode.OK)
        //        throw new Exception(response.StatusDescription);

        //    if(response.Data.access_token == null)
        //        throw new Exception(response.Content);


        //    return response.Data.access_token;
        //}
    //}


}