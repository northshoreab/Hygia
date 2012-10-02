using System;

namespace Hygia.API.Infrastructure.Authentication.GitHubDomain
{
    public class GitHubUser
    {
        public string Login { get; set; }
        public int Id { get; set; }
        public string Avatar_Url { get; set; }
        public string Gravatar_Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Blog { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public bool Hireable { get; set; }
        public string Bio { get; set; }
        public int Public_Repos { get; set; }
        public int Public_gists { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public string Html_Url { get; set; }
        public DateTime Created_At { get; set; }
        public string Type { get; set; }
        public int Total_Private_Repos { get; set; }
        public int Owned_Private_Repos { get; set; }
        public int Private_Gists { get; set; }
        public int Disk_Usage { get; set; }
        public int Collaborators { get; set; }
        public int MyProperty { get; set; }
        public GitHubPlan Plan { get; set; }
    }
}