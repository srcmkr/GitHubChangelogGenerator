using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GitHubChangelogGeneratorLib.Models.GitHub
{
    public class GitHubCommit
    {
        public string Sha { get; set; }
        public GitHubCommitDetails Commit { get; set; }

        public string GetShortHash()
        {
            if (!string.IsNullOrEmpty(Sha) && Sha.Length >= 7)
            {
                return Sha.Substring(0,7);
            }
            return "NOTHASH";
        }

        public string GetCommitMessage()
        {
            // Updated readme.md\n\nAdding two issues #2 and #1
            if (string.IsNullOrWhiteSpace(Commit.Message))
                return string.Empty;

            string pattern = @"(.*)\n\n";
            var found = Regex.Matches(Commit.Message, pattern, RegexOptions.Multiline);
            if (found.Count > 0)
                return found[0].Groups[1].Value;
            else
                return Commit.Message;
        }

        public List<int> GetIssueIds()
        {
            // Updated readme.md\n\nAdding two issues #2 and #1
            var foundIds = new List<int>();
            if (string.IsNullOrWhiteSpace(Commit.Message))
                return foundIds;

            string pattern = @"#(\d{1,10})";
            foreach (Match m in Regex.Matches(Commit.Message, pattern))
            {
                var numberOnly = m.Value.Replace("#", "");
                if (int.TryParse(numberOnly, out int foundId))
                {
                    if (foundId > 0) foundIds.Add(foundId);
                }
            }

            return foundIds;
        }
    }
}
