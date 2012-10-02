namespace Hygia.API.Infrastructure.Authentication.GitHubDomain
{
    public class GitHubPlan
    {
        public string Name { get; set; }
        public int Space { get; set; }
        public int Collaborators { get; set; }
        public int Private_Repos { get; set; }
    }
}