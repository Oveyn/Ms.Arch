using System;
using System.Text.Json.Serialization;

namespace Ms.Arch.Hw02.Api.CommonCore.VersionsInfo
{
    public sealed class CommonVersionResponse
    {
        [JsonPropertyName("dotnet_running_in_container")]
        public bool DotnetRunningInContainer { get; set; }

        [JsonPropertyName("application_name")]
        public string ApplicationName { get; set; }

        [JsonPropertyName("application_version")]
        public string ApplicationVersion { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("start_date")]
        public DateTimeOffset StartDateTime { get; set; }


    }
}
