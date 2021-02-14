namespace GitHubChangelogGeneratorLib.Models.GitHub
{
    public class GitHubCommitDetails
    {
        public string Message { get; set; }
        public GitHubCommitter Committer { get; set; }
    }
}
