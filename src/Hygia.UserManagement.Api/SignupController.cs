using System.Collections.Generic;

namespace Hygia.UserManagement.Api
{
    using System;
    using Core;
    using FubuMVC.Core;
    using NServiceBus;
    using Operations.Email.Commands;
    using Raven.Client;

    public class SignUpController
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        [JsonEndpoint]
        public dynamic post_signup(SignUpInputModel model)
        {
            var userId = model.Email.ToGuid();

            var account = Session.Load<UserAccount>(userId);

            
            if(account != null)//todo- add a behaviour that translates exceptions to json that backbone can use
                throw new InvalidOperationException("A user account for " + model.Email + " already exists");
            
            account = new UserAccount
                          {
                              Id = userId,
                              UserName = model.Email,
                              SignedUpAt = DateTime.UtcNow,
                              Status = UserAccountStatus.Unverified
                          };

            Session.Store(account);

            Bus.Send(new SendEmailRequest
                         {
                             DisplayName = "WatchR - SignUp",
                             To = model.Email,
                             Subject = "Please verify your email at WatchR.se",
                             Body = "Todo",
                             Service = "usermanagement",
                             Parameters = userId.ToString()
                         });
            return account;
        }
    }

    public enum UserAccountStatus
    {
        Unverified
    }

    public class UserAccount
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime SignedUpAt { get; set; }

        public UserAccountStatus Status { get; set; }
    }

    public class SignUpInputModel
    {
        public string Email { get; set; }
    }
}
