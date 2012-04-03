using System.Collections.Generic;

namespace Hygia.UserManagement.Api
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Core;
    using FubuMVC.Core;
    using NServiceBus;
    using Operations.Email.Commands;
    using Raven.Client;
    using RestSharp;

    public class SignUpController
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }
        [JsonEndpoint]
        public dynamic post_signup_verify(VerifyInputModel model)
        {
            var account = Session.Load<UserAccount>(model.UserId);


            if (account == null)
                throw new InvalidOperationException("No user account found for userId " + model.UserId);

            account.Status = UserAccountStatus.Verified;

            return account;
        }

        [JsonEndpoint]
        public dynamic post_signup_github(GithubSignUpInputModel model)
        {
            var accessToken = GetGithubAccessToken(model.Code);

            //get user details so that we can auto suggest repos etc
            var userDetailsRequest = new RestRequest("/user", Method.GET) { RequestFormat = DataFormat.Json };

            userDetailsRequest.AddParameter("access_token", accessToken);

            var client = new RestClient("https://api.github.com");

            var response = client.Execute<GithubUserResponse>(userDetailsRequest);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(response.StatusDescription);

            var githubUserId = response.Data.id;

            var account = Session.Query<UserAccount>().SingleOrDefault(u => u.Email == response.Data.email);


            //see if we alrady has this user (by email)
            if (account != null && account.GithubUserId != githubUserId)
            {
                if (account.GithubUserId != null)
                    throw new InvalidOperationException("Account with the same email already exists and is associated with another Github account");

                account.GithubUserId = response.Data.id;
                account.Status = UserAccountStatus.Verified;

                return account;
            }

            //see if we already has this user (by github id)
            account = Session.Query<UserAccount>().SingleOrDefault(u => u.GithubUserId == githubUserId);

            if (account != null)
                return account;

            var userId = response.Data.email.ToGuid();

            account = new UserAccount
            {
                Id = userId,
                UserName = response.Data.email,
                Email = response.Data.email,
                SignedUpAt = DateTime.UtcNow,
                Status = UserAccountStatus.Verified,
                GithubUserId = githubUserId,
                GravatarId = response.Data.gravatar_id
            };

            Session.Store(account);

           

            return account;
        }

        //todo - move this method to a behaviour so this can be reused for the login as well
        static string GetGithubAccessToken(string code)
        {
            var client = new RestClient("https://github.com");

            var accessTokenRequest = new RestRequest("/login/oauth/access_token", Method.POST) { RequestFormat = DataFormat.Json };

            accessTokenRequest.AddParameter("client_id", "933251074a0f47066f44");
            accessTokenRequest.AddParameter("client_secret", "11c5684e3f03e9efd49d3c7b663dcc0d36cf6bda");
            accessTokenRequest.AddParameter("code", code);

            var response = client.Execute<AccessTokenResponse>(accessTokenRequest);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(response.StatusDescription);

            return response.Data.access_token;
        }

        [JsonEndpoint]
        public dynamic post_signup(SignUpInputModel model)
        {
            var userId = model.Email.ToGuid();

            var account = Session.Load<UserAccount>(userId);


            if (account != null)//todo- add a behaviour that translates exceptions to json that backbone can use
                throw new InvalidOperationException("A user account for " + model.Email + " already exists");

            account = new UserAccount
                          {
                              Id = userId,
                              UserName = model.Email,
                              Email = model.Email,
                              SignedUpAt = DateTime.UtcNow,
                              Status = UserAccountStatus.Unverified
                          };

            Session.Store(account);

            Bus.Send(new SendEmailRequest
                         {
                             DisplayName = "WatchR - SignUp",
                             To = model.Email,
                             Subject = "Please verify your email at WatchR.se",
                             Body = "http://watchr.se/#verify/" + userId,
                             Service = "usermanagement",
                             Parameters = userId.ToString()
                         });
            return account;
        }
    }

    public class GithubUserResponse
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string gravatar_id { get; set; }
        public string login { get; set; }
    }

    public class AccessTokenResponse
    {
        public string access_token { get; set; }
    }

    public class GithubSignUpInputModel
    {
        public string Code { get; set; }
    }

    public class VerifyInputModel
    {
        public Guid UserId { get; set; }
    }

    public enum UserAccountStatus
    {
        Unverified,
        Verified
    }

    public class UserAccount
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime SignedUpAt { get; set; }

        public UserAccountStatus Status { get; set; }

        public string GithubUserId { get; set; }

        public string Email { get; set; }

        public string GravatarId { get; set; }
    }

    public class SignUpInputModel
    {
        public string Email { get; set; }
    }
}
