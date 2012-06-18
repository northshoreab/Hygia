using System;
using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure.Authentication;
using Hygia.API.Models.UserManagement.UserAccounts;
using Hygia.Core;
using Hygia.UserManagement.Domain;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using IdentityProvider = Hygia.UserManagement.Domain.IdentityProvider;
using UserAccount = Hygia.UserManagement.Domain.UserAccount;

namespace Hygia.API.Controllers.Operations.Authentication
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/signup/withgithub")]
    public class SignupWithGithubController : ApiController
    {
        private readonly IDocumentSession _session;

        public SignupWithGithubController(IDocumentSession session)
        {
            _session = session;
        }

        public Models.UserManagement.UserAccounts.UserAccount Get()
        {
            var user = User.Identity as IClaimsIdentity;
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

            return account.ToOutputModel();
        }
    }
}