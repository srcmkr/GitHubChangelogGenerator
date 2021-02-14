using GitHubChangelogGeneratorLib.Models.GitHub;
using System;
using System.Collections.Generic;

namespace GitHubChangelogGeneratorLib.Models.ChangelogGenerator
{
    public class ChangelogIssue
    {
        public string Caption { get; set; }
        public List<GitHubLabel> Labels { get; set; }
        public string Description { get; set; }
        public long Number { get; set; }
    }
}
