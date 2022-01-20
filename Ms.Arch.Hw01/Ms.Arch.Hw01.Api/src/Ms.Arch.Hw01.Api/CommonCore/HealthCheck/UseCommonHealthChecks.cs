using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace Ms.Arch.Hw01.Api.CommonCore.HealthCheck
{
    public static class UseCommonHealthChecks
    {
        public static void UseCommonHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("Health"),
                ResponseWriter = async (context, report) =>
                {
                    var result = new HealthCheckResponse(report);
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var resultJson = CommonJsonSerializer.Serialize(result);
                    await context.Response.WriteAsync(resultJson);
                }
            });
        }
    }
}
