using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ms.Arch.Hw01.Api.CommonCore.HealthCheck
{
    public class HealthCheckResponse
    {
        /// <summary>
        /// Need to hw01
        /// </summary>
        /// <param name="report"></param>
        public HealthCheckResponse(HealthReport report)
        {
            if (report.Status == HealthStatus.Healthy)
                Status = "OK";
            else
                Status = report.Status.ToString();
        }

        [JsonPropertyName("status")] public string Status { get; }
    }
}