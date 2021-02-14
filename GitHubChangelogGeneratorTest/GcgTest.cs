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

            System.Console.WriteLine(@"
 _____ _ _   _   _       _     _____ _                            _             
|  __ (_) | | | | |     | |   /  __ \ |                          | |            
| |  \/_| |_| |_| |_   _| |__ | /  \/ |__   __ _ _ __   __ _  ___| | ___   __ _ 
| | __| | __|  _  | | | | '_ \| |   | '_ \ / _` | '_ \ / _` |/ _ \ |/ _ \ / _` |
| |_\ \ | |_| | | | |_| | |_) | \__/\ | | | (_| | | | | (_| |  __/ | (_) | (_| |
 \____/_|\__\_| |_/\__,_|_.__/ \____/_| |_|\__,_|_| |_|\__, |\___|_|\___/ \__, |
 _____                           _                      __/ |              __/ | 
|  __ \                         | |                    |___/              |___/ 
| |  \/ ___ _ __   ___ _ __ __ _| |_ ___  _ __                                  
| | __ / _ \ '_ \ / _ \ '__/ _` | __/ _ \| '__|     
| |_\ \  __/ | | |  __/ | | (_| | || (_) | |        Available on github.com:
 \____/\___|_| |_|\___|_|  \__,_|\__\___/|_|        srcmkr/GitHubChangelogGenerator

                                                                                
Environment-Variables:
");

            System.Console.WriteLine($"REPOSITORY:");
            System.Console.WriteLine($"  GITUSERNAME: {EnvironmentHelper.GetEnvironmentVariable("GITUSERNAME")}");
            System.Console.WriteLine($"  GITREPOSITORY: {EnvironmentHelper.GetEnvironmentVariable("GITREPOSITORY")}");
            System.Console.WriteLine($"  BRANCH: {EnvironmentHelper.GetEnvironmentVariable("BRANCH")}");
            System.Console.WriteLine($"");
            System.Console.WriteLine($"CHANGELOG:");
            System.Console.WriteLine($"  CHANGELOGNAME: {EnvironmentHelper.GetEnvironmentVariable("CHANGELOGNAME")}");
            System.Console.WriteLine($"  CHANGELOGLABEL: {EnvironmentHelper.GetEnvironmentVariable("CHANGELOGLABEL")}");
            System.Console.WriteLine($"  CHANGELOGPUBLISHLABELS: {EnvironmentHelper.GetEnvironmentVariable("CHANGELOGPUBLISHLABELS")}");
            System.Console.WriteLine($"  TEMPLATE: {EnvironmentHelper.GetEnvironmentVariable("TEMPLATE")}");
            System.Console.WriteLine($"");
            System.Console.WriteLine($"SECURITY:");
            if (string.IsNullOrEmpty(EnvironmentHelper.GetEnvironmentVariable("PAT")))
                System.Console.WriteLine($"   PAT IS NOT SET");
            else
                System.Console.WriteLine($"   PAT IS SET: {EnvironmentHelper.GetEnvironmentVariable("PAT").Substring(0,5)}*REDACTED*");

            System.Console.WriteLine($"");
            System.Console.WriteLine($"Generating changelog...");
            var htmlFileContent = await new GitHubChangelogGenerator(settings, repo, creds).CreateHtmlTemplate(EnvironmentHelper.GetEnvironmentVariable("TEMPLATE").ToLower());
            File.WriteAllText("data/output.html", htmlFileContent);
            System.Console.WriteLine($"Changelog generated and saved to data/output.html");
        }
    }
}
