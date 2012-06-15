using System;
using System.Collections.Generic;
using Hygia.UserManagement.Domain;

namespace Hygia.API.Models.UserManagement.UserAccounts
{
    public class UserAccount
    {
        public UserAccount()
        {
            IdentityProviders = new List<IdentityProvider>();
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime SignedUpAt { get; set; }

        public UserAccountStatus Status { get; set; }

        public string Email { get; set; }
        public string GravatarId { get; set; }

        public IList<IdentityProvider> IdentityProviders { get; set; }
    }

    public class IdentityProvider
    {
        public string UserId { get; set; }
        public string Issuer { get; set; }
    }
}
