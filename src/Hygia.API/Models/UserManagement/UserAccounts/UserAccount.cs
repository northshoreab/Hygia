using System;
using Hygia.UserManagement.Domain;

namespace Hygia.API.Models.UserManagement.UserAccounts
{
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
}
