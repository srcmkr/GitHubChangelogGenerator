using GitHubChangelogGeneratorLib.Models.ChangelogGenerator;
using GitHubChangelogGeneratorLib.Models.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitHubChangelogGeneratorLib.templates
{
    public class DayTemplate
    {
        private ChangelogSettings Settings;
        private List<ChangelogCommit> TemplateContent;
        private List<GitHubLabel> AllGitHubLabels;

        public DayTemplate(ChangelogSettings settings, List<ChangelogCommit> templateContent, List<GitHubLabel> allGitHubLabels)
        {
            Settings = settings;
            TemplateContent = templateContent;
            AllGitHubLabels = allGitHubLabels;
        }

        public string GenerateContent()
        {
            if (TemplateContent == null) return default;

            var html = @"
<html>
<head>
    <title>{{CAPTION}}</title>
    <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css' crossorigin='anonymous'>
    <style>
body {
  font-size: 16px;
}
.badge {
    padding: 5px 10px;
}
ul.timeline {
    list-style-type: none;
    position: relative;
}
ul.timeline:before {
    content: ' ';
    background: #d4d9df;
    display: inline-block;
    position: absolute;
    left: 16px;
    width: 2px;
    height: 100%;
    z-index: 400;
}
ul.timeline > li {
    margin: 20px 0;
    padding-left: 20px;
}
ul.timeline > li:before {
    content: ' ';
    background: white;
    display: inline-block;
    position: absolute;
    border-radius: 50%;
    border: 3px solid #22c0e8;
    left: 10px;
    width: 15px;
    height: 15px;
    z-index: 400;
}
.entry {
    display: block;
    padding: 4px;
}
.issueId {
    color: #ccc;
}
.caption {
    margin-left: 25px;
}
.badge {
    margin-right: 10px;
}
</style>
</head>
<body>
    <h2 class='caption'>{{CAPTION}}</h2>
    {{TABLE}}
</body>
</html>
";
            html = html.Replace("{{CAPTION}}", Settings.Caption);
            var sb = new StringBuilder();
            sb.AppendLine("<ul class='timeline'>");

            var commitDays = new List<CommitsPerDateTime>();
            foreach (var commit in TemplateContent)
            {
                var foundDay = commitDays.SingleOrDefault(c => c.Date == commit.Date.Date);
                if (foundDay == null)
                    commitDays.Add(new CommitsPerDateTime
                    {
                        Date = commit.Date.Date,
                        Commits = new List<ChangelogCommit>() { commit }
                    });
                else
                    foundDay.Commits.Add(commit);
            }

            foreach (var day in commitDays)
            {
                var wasAdded = false;
                var subSb = new StringBuilder();
                subSb.AppendLine("<li>");
                subSb.AppendLine("<p>");
                subSb.AppendLine($"<strong>Changelog {day.Date:dd.MM.yyyy}</strong>");

                foreach (var label in AllGitHubLabels.Where(c => c.Name.ToLower() != Settings.ChangelogLabel.ToLower()).ToList())
                {
                    foreach (var commit in day.Commits)
                    {
                        var issues = commit.Issues.Where(c => c.Labels.Any(d => d.Name.ToLower() == label.Name.ToLower())).ToList();
                        if (issues.Count > 0)
                        {
                            foreach (var issue in issues)
                            {
                                if (issue.Labels.Any(c => c.Name.ToLower() == Settings.ChangelogLabel.ToLower()))
                                {
                                    subSb.AppendLine($"<div class='entry'><span class='badge' style='background-color: #{label.Color};'>{label.Name}</span> {issue.Caption} <span class='issueId'>(#{issue.Number})</span></div>");
                                    wasAdded = true;
                                }

                            }
                        }
                    }
                }
                subSb.AppendLine("</p>");
                subSb.AppendLine("</li>");

                if (wasAdded)
                    sb.Append(subSb);
            }


            sb.AppendLine("</ul>");
            html = html.Replace("{{TABLE}}", sb.ToString());
            return html;
        }
    }

    public class CommitsPerDateTime
    {
        public DateTime Date { get; set; }
        public List<ChangelogCommit> Commits { get; set; }
    }
}
