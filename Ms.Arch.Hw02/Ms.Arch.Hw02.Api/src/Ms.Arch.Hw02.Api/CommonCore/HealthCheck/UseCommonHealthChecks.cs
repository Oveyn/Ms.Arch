using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace Ms.Arch.Hw02.Api.CommonCore.HealthCheck
{
    public static class UseCommonHealthChecks
    {
        public static void UseCommonHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(HealthCheckTags.Liveness),
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var resultJson = CommonJsonSerializer.Serialize(report);
                    await context.Response.WriteAsync(resultJson);
                }
            });

            app.UseHealthChecks("/readiness", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(HealthCheckTags.Readiness),
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var resultJson = CommonJsonSerializer.Serialize(report);
                    await context.Response.WriteAsync(resultJson);
                }
            });
        }
    }
}
