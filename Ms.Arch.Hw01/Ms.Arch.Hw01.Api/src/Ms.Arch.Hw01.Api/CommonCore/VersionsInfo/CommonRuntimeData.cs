using System;
using System.Reflection;

namespace Ms.Arch.Hw01.Api.CommonCore.VersionsInfo
{
    public static class CommonRuntimeData
    {
        public static DateTimeOffset StartDateTime { get; private set; }
        public static string ApplicationName { get; private set; } = String.Empty;
        public static string ApplicationVersion { get; private set; } = String.Empty;

        public static void Initialize(Assembly applicationAssembly)
        {
            StartDateTime = GetUtcNow();
            InitializeFromAssemblies(applicationAssembly);
        }

        private static DateTimeOffset GetUtcNow()
        {
            var utcNow = DateTimeOffset.UtcNow;
            return utcNow.AddTicks(-(utcNow.Ticks % TimeSpan.TicksPerSecond));
        }

        private static void InitializeFromAssemblies(Assembly applicationAssembly)
        {
            ApplicationName = applicationAssembly.GetName().Name ?? String.Empty;
            ApplicationVersion = GetAssemblyVersion(applicationAssembly);
        }

        private static string GetAssemblyVersion(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute != null && !String.IsNullOrEmpty(attribute.InformationalVersion))
                return attribute.InformationalVersion;

            var assemblyName = assembly.GetName();
            var assemblyVersion = assemblyName.Version;

            return assemblyVersion?.ToString(3) ?? String.Empty;
        }
    }
}
