using System;
using System.Diagnostics;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    public sealed class AppMetricsCollector : AspNetCoreDiagnosticObserver
    {
        private readonly AppMetrics _appMetrics;

        public AppMetricsCollector(
            AppMetrics appMetrics)
        {
            _appMetrics = appMetrics;
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn")]
        public void MicrosoftAspNetCoreHostingHttpRequestIn()
        {
            Console.WriteLine("MicrosoftAspNetCoreHostingHttpRequestIn");
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")]
        public void MicrosoftAspNetCoreHostingHttpRequestInStart(HttpContext httpContext)
        {
            CollectorData.Current = new DiagnosticCollectorData
            {
                CorrelationId = Guid.NewGuid(),
                Stopwatch = Stopwatch.StartNew()
            };
        }

        [DiagnosticName("Microsoft.AspNetCore.Routing.EndpointMatched")]
        public void MicrosoftAspNetCoreRoutingEndpointMatched(HttpContext httpContext)
        {
            CollectorData.Current.Stopwatch = Stopwatch.StartNew();

            var endpointFeature = httpContext.Features.Get<IEndpointFeature>();
            var endpoint = endpointFeature.Endpoint;

            if (!(endpoint is RouteEndpoint routeEndpoint))
                return;

            var metadata = routeEndpoint.Metadata.GetMetadata<GrpcMethodMetadata>();
            if (metadata != null)
            {
                var type = metadata.Method.Type;
                var fullName = metadata.Method.FullName;
                var handler = $"GRPC {fullName} ({type})";

                CollectorData.Current.Protocol = "grpc";
                CollectorData.Current.Handler = handler;
            }
            else
            {
                var method = httpContext.Request.Method;
                var routePattern = routeEndpoint.RoutePattern.RawText;
                var handler = $"HTTP {method} /{routePattern}";

                CollectorData.Current.Protocol = "http";
                CollectorData.Current.Handler = handler;
            }

            _appMetrics.RequestCount
                .WithLabels(
                    CollectorData.Current.ServiceName,
                    CollectorData.Current.ServiceVersion,
                    CollectorData.Current.Handler,
                    CollectorData.Current.Protocol
                )
                .Inc();
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")]
        public void MicrosoftAspNetCoreHostingHttpRequestInStop(HttpContext httpContext)
        {
            CollectorData.Current.Stopwatch.Stop();
            var duration = CollectorData.Current.Stopwatch.Elapsed.TotalSeconds;
            var statusCode = httpContext.Response.StatusCode.ToString();

            if (CollectorData.Current.Handler is null)
            {
                return;
            }

            _appMetrics.ResponseTimeSeconds
                .WithLabels(
                    CollectorData.Current.ServiceName,
                    CollectorData.Current.ServiceVersion,
                    CollectorData.Current.Handler ?? String.Empty,
                    CollectorData.Current.Protocol ?? String.Empty,
                    statusCode ?? String.Empty
                )
                .Observe(duration);
        }
    }
}