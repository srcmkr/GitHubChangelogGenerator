using System.Threading.Tasks;

namespace GitHubChangelogGeneratorDocker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GcgTest.Test();
        }
    }
}
