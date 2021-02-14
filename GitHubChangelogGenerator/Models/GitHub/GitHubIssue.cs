using System.Collections.Generic;

namespace GitHubChangelogGeneratorLib.Models.GitHub
{
    public class GitHubIssue
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public string Title { get; set; }
        public List<GitHubLabel> Labels { get; set; }
        public string State { get; set; }
        public string Body { get; set; }
    }
}
