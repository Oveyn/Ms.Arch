using System;

namespace Ms.Arch.Hw01.Api.CommonCore
{
    public static class CommonEnvironment
    {
        public static bool DotnetRunningInContainer { get; } =
            GetBooleanEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

        public static string Hostname { get; } =
            Environment.GetEnvironmentVariable("HOSTNAME") ?? String.Empty;

        private static bool GetBooleanEnvironmentVariable(string variable, bool defaultResult = false)
        {
            var value = Environment.GetEnvironmentVariable(variable);
            var success = Boolean.TryParse(value, out var result);

            return success ? result : defaultResult;
        }
    }
}
