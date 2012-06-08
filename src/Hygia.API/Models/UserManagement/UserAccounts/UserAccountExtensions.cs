using System.Collections.Generic;
using System.Linq;

namespace Hygia.API.Models.UserManagement.UserAccounts
{
    public static class UserAccountExtensions
    {
        public static IEnumerable<UserAccount> ToOutputModel(this IEnumerable<Hygia.UserManagement.Domain.UserAccount> domainUserAccounts)
        {
            return domainUserAccounts.Select(ToOutputModel);
        }

        public static UserAccount ToOutputModel(this Hygia.UserManagement.Domain.UserAccount domainUserAccount)
        {
            return new UserAccount
                       {
                           Email = domainUserAccount.Email,
                           Id = domainUserAccount.Id,
                           GithubUserId = domainUserAccount.GithubUserId,
                           GravatarId = domainUserAccount.GravatarId,
                           SignedUpAt = domainUserAccount.SignedUpAt,
                           Status = domainUserAccount.Status,
                           UserName = domainUserAccount.UserName
                       };
        }
    }
}