using GitHubChangelogGeneratorLib.Models.GitHub;
using System;
using System.Collections.Generic;

namespace GitHubChangelogGeneratorLib.Models.ChangelogGenerator
{
    public class ChangelogCommit
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public List<ChangelogIssue> Issues { get; set; }
        public DateTime Date { get; set; }
    }
}
