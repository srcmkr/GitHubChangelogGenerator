using System.Threading.Tasks;

namespace GitHubChangelogGeneratorDocker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var gcgt = new GcgTest();
            await GcgTest.Test();
        }
    }
}
