using GitHubChangelogGeneratorLib;
using GitHubChangelogGeneratorLib.Models.ChangelogGenerator;
using GitHubChangelogGeneratorLib.Models.GitHub;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubChangelogGeneratorDocker
{
    public class GcgTest
    {
        public static async Task Test()
        {
            var repo = new GitHubRepository
            {
                Branch = EnvironmentHelper.GetEnvironmentVariable("BRANCH"),
                Username = EnvironmentHelper.GetEnvironmentVariable("GITUSERNAME"),
                Repository = EnvironmentHelper.GetEnvironmentVariable("GITREPOSITORY")
            };

            var creds = new GitHubPATAuthentification
            {
                PersonalAccessToken = EnvironmentHelper.GetEnvironmentVariable("PAT")
            };

            var settings = new ChangelogSettings
            {
                Caption = EnvironmentHelper.GetEnvironmentVariable("CHANGELOGNAME"),
                ChangelogLabel = EnvironmentHelper.GetEnvironmentVariable("CHANGELOGLABEL").ToLower(),
                ChangelogPublishLabels = EnvironmentHelper.GetEnvironmentVariable("CHANGELOGPUBLISHLABELS").ToLower().Split(',').ToList()
            };

            var htmlFileContent = await new GitHubChangelogGenerator(settings, repo, creds).CreateHtmlTemplate("day");
            File.WriteAllText("data/output.html", htmlFileContent);
        }
    }
}
