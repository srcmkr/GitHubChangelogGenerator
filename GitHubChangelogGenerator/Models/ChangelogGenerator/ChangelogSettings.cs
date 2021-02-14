using System.Collections.Generic;

namespace GitHubChangelogGeneratorLib.Models.ChangelogGenerator
{
    public class ChangelogSettings
    {
        public string Caption { get; set; }
        public string ChangelogLabel { get; set; }
        public List<string> ChangelogPublishLabels { get; set; }
    }
}
