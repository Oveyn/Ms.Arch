using Prometheus.Client;

namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    public sealed class AppMetrics
    {
        public IMetricFamily<ICounter<long>> RequestCount { get; }
        public IMetricFamily<IHistogram> ResponseTimeSeconds { get; }

        public AppMetrics(IMetricFactory metricFactory)
        {
            var labelsBase = new[] {"service_name", "service_version", "handler", "protocol"};
            RequestCount =
                metricFactory.CreateCounterInt64(
                    "app_request_total",
                    string.Empty,
                    true,
                    labelsBase);

            var labelsRTS = new[] {"service_name", "service_version", "handler", "protocol", "status_code"};
            ResponseTimeSeconds = metricFactory.CreateHistogram(
                "app_response_time_seconds",
                string.Empty,
                new[] {0.001, 0.003, 0.007, 0.015, 0.05, 0.1, 0.2, 0.5, 1, 2, 5},
                labelsRTS);
        }
    }
}