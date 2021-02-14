using GitHubChangelogGeneratorLib.Models.ChangelogGenerator;
using GitHubChangelogGeneratorLib.Models.GitHub;
using GitHubChangelogGeneratorLib.templates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GitHubChangelogGeneratorLib
{
    public class GitHubChangelogGenerator
    {
        private GitHubRepository Repository { get; set; }
        private GitHubPATAuthentification PatAuth { get; set; }
        private List<GitHubLabel> AllGitHubLabels { get; set; }
        private List<GitHubIssue> AllGitHubIssues { get; set; }
        private ChangelogSettings Settings { get; set; }

        private HttpClient Client { get; set; }

        public GitHubChangelogGenerator(ChangelogSettings settings, GitHubRepository repository, GitHubPATAuthentification patAuth = null)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com")
            };

            Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitHubChangelogGenerator", "1.0"));
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Settings = settings;

            if (patAuth != null)
            {
                PatAuth = patAuth;
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", PatAuth.PersonalAccessToken);
            }

            if (repository != null)
            {
                Repository = repository;
            }
            else
            {
                throw new ArgumentNullException("Please provide GitHub Repository information");
            }
        }

        private async Task<List<GitHubCommit>> GetAllCommits()
        {
            var getCommitsRequest = await Client.GetAsync($"/repos/{Repository.Username}/{Repository.Repository}/commits?sha={Repository.Branch}");
            if (!getCommitsRequest.IsSuccessStatusCode) return default;

            var content = await getCommitsRequest.Content.ReadAsStringAsync();

            var allCommits = JsonConvert.DeserializeObject<List<GitHubCommit>>(content);
            return allCommits;
        }

        private async Task<List<GitHubIssue>> GetAllIssues()
        {
            var getIssuesRequest = await Client.GetAsync($"/repos/{Repository.Username}/{Repository.Repository}/issues?state=all");

            if (!getIssuesRequest.IsSuccessStatusCode) return default;

            var content = await getIssuesRequest.Content.ReadAsStringAsync();

            var allIssues = JsonConvert.DeserializeObject<List<GitHubIssue>>(content);
            return allIssues;
        }

        private async Task<List<GitHubLabel>> GetAllLabels()
        {
            var getLabelsRequest = await Client.GetAsync($"/repos/{Repository.Username}/{Repository.Repository}/labels");
            if (!getLabelsRequest.IsSuccessStatusCode) return default;

            var content = await getLabelsRequest.Content.ReadAsStringAsync();

            var allLabels = JsonConvert.DeserializeObject<List<GitHubLabel>>(content);
            var filteredLabels = new List<GitHubLabel>();

            foreach(var changelogPublishLabel in Settings.ChangelogPublishLabels)
            {
                var labelToAdd = allLabels.SingleOrDefault(c => c.Name.ToLower() == changelogPublishLabel.ToLower());
                if (labelToAdd != null)
                    filteredLabels.Add(labelToAdd);
            }

            var changelogLabel = allLabels.SingleOrDefault(c => c.Name.ToLower() == Settings.ChangelogLabel.ToLower());
            if (changelogLabel == null)
                return default;
            else filteredLabels.Add(changelogLabel);

            return filteredLabels;
        }

        private List<ChangelogIssue> GetChangelogIssues()
        {
            var changelogLabel = AllGitHubLabels.SingleOrDefault(c => c.Name.ToLower().Equals(Settings.ChangelogLabel.ToLower()));
            if (changelogLabel == null) return default;

            var changelogIssues = new List<ChangelogIssue>();
            foreach (var issue in AllGitHubIssues)
            {
                if (issue.Labels.Any(c => c.Name.ToLower() == changelogLabel.Name.ToLower()))
                {
                    changelogIssues.Add(new ChangelogIssue
                    {
                        Number = issue.Number,
                        Caption = issue.Title,
                        Labels = issue.Labels,
                        Description = issue.Body
                    });
                }

            }

            return changelogIssues;
        }

        public async Task<List<ChangelogCommit>> GetChangelogCommits()
        {
            AllGitHubLabels = await GetAllLabels();
            AllGitHubIssues = await GetAllIssues();

            var issues = GetChangelogIssues();
            if (issues == null || issues.Count <= 0) return default;

            var changelogCommits = new List<ChangelogCommit>();
            var githubCommits = await GetAllCommits();
            if (githubCommits != null)
            {
                foreach (var commit in githubCommits)
                {
                    var changelogCommit = new ChangelogCommit
                    {
                        Id = commit.GetShortHash(),
                        Message = commit.GetCommitMessage(),
                        Issues = new List<ChangelogIssue>(),
                        Date = commit.Commit.Committer.Date,
                    };

                    var issueIds = commit.GetIssueIds();
                    foreach (var issueId in issueIds)
                    {
                        var issueToAdd = AllGitHubIssues.SingleOrDefault(c => c.Number == issueId);
                        if (issueToAdd != null)
                        {
                            var changelogIssueToAdd = new ChangelogIssue
                            {
                                Number = issueToAdd.Number,
                                Caption = issueToAdd.Title,
                                Description = issueToAdd.Body,
                                Labels = issueToAdd.Labels
                            };

                            changelogCommit.Issues.Add(changelogIssueToAdd);
                        }
                    }

                    changelogCommits.Add(changelogCommit);
                }
            }

            return changelogCommits;
        }

        public async Task<string> CreateHtmlTemplate(string template = "default")
        {
            var templateContent = await GetChangelogCommits();

            switch (template.ToLower())
            {
                case "day":
                    var dayEngine = new DayTemplate(Settings, templateContent, AllGitHubLabels);
                    return dayEngine.GenerateContent();
                default:
                    var defaultEngine = new DefaultTemplate(Settings, templateContent, AllGitHubLabels);
                    return defaultEngine.GenerateContent();
            }
        }
    }
}
