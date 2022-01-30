using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ms.Arch.Hw02.Api.CommonCore.CommonMiddleware;
using Ms.Arch.Hw02.Api.CommonCore.Metrics;
using Prometheus.Client.AspNetCore;

namespace Ms.Arch.Hw02.Api.CommonCore
{
    public static class CommonExtensions
    {
        public static void UseCommonVersion(this IApplicationBuilder app)
        {
            app.UseCommonMiddleware<CommonVersionMiddleware>("/version");
        }

        public static void UseCommonMetrics(this IApplicationBuilder app)
        {
            var diagnosticObservers = app
                .ApplicationServices.GetServices<DiagnosticObserverBase>();
            foreach (var diagnosticObserver in diagnosticObservers)
            {
                DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
            }

            app.UsePrometheusServer(options =>
            {
                options.MapPath = "/metrics";
                options.UseDefaultCollectors = true;
            });
        }

        private static void UseCommonMiddleware<TMiddleware>(this IApplicationBuilder app, string path)
        {
            app.Map(path, b => b.UseMiddleware<TMiddleware>());
        }
    }
}
