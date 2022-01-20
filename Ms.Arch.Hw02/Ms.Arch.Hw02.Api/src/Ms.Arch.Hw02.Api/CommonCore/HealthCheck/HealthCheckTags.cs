namespace Ms.Arch.Hw02.Api.CommonCore.HealthCheck
{
    internal static class HealthCheckTags
    {
        public static string Liveness { get; } = nameof(Liveness);
        public static string Readiness { get; } = nameof(Readiness);
    }
}
