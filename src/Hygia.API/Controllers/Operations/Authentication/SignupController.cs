using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure.Authentication;
using Hygia.Core;
using Hygia.UserManagement.Domain;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using IdentityProvider = Hygia.UserManagement.Domain.IdentityProvider;
using UserAccount = Hygia.UserManagement.Domain.UserAccount;

namespace Hygia.API.Controllers.Operations.Authentication
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/signup")]
    [Authorize]
    public class SignupController : ApiController
    {
        private readonly IDocumentSession _session;

        public SignupController(IDocumentSession session)
        {
            _session = session;
        }

        public HttpResponseMessage Put()
        {
            var user = User.Identity as IClaimsIdentity;

            //TODO: get user info from claims instead
            var githubUserResponse = GithubHelper.GetGithubUser(user.GetClaimValue(Constants.ClaimTypes.GithubAccessToken));
            var userId = githubUserResponse.id.ToGuid();

            var account = new UserAccount
                              {
                                  Id = userId,
                                  UserName = githubUserResponse.email,
                                  Email = githubUserResponse.email,
                                  SignedUpAt = DateTime.UtcNow,
                                  Status = UserAccountStatus.Verified,
                                  GravatarId = githubUserResponse.gravatar_id,
                                  IdentityProviders = new List<IdentityProvider>
                                                          {
                                                              new IdentityProvider{Issuer = Constants.Issuers.Github, UserId = githubUserResponse.id}
                                                          }
                              };

            _session.Store(account);

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Headers.Location = new Uri("/api/users/" + account.Id);

            return response;
        }
    }
}