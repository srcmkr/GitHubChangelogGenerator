using System;

namespace GitHubChangelogGeneratorDocker
{
    public static class EnvironmentHelper
    {
        public static string GetEnvironmentVariable(string varName)
        {
            varName = varName.ToUpper();

            var processVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrEmpty(processVar)) return processVar;

            var userVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(userVar)) return userVar;

            var machineVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(machineVar)) return machineVar;

            return string.Empty;
        }
    }
}
