using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prometheus.Client;
using Prometheus.Client.Collectors;

namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    public static class CommonPrometheusExtension
    {
        public static void AddCommonPrometheus(this IServiceCollection services)
        {
            services.AddSingleton<ICollectorRegistry, CollectorRegistry>();
            services.AddSingleton<IMetricFactory, MetricFactory>();

            services.TryAddTransient<AppMetrics>();
            services.AddDiagnosticObserver<AppMetricsCollector>();
        }
    }
}
