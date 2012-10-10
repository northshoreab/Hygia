using System;

namespace Hygia.API.Models.UserManagement.UserAccounts
{
    public class Me
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
    }
}