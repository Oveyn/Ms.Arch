using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ms.Arch.Hw01.Api.CommonCore.VersionsInfo;

namespace Ms.Arch.Hw01.Api.CommonCore.CommonMiddleware
{
    internal class CommonVersionMiddleware
    {
        public CommonVersionMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            var response = new CommonVersionResponse
            {
                DotnetRunningInContainer = CommonEnvironment.DotnetRunningInContainer,
                Hostname = CommonEnvironment.Hostname,
                ApplicationName = CommonRuntimeData.ApplicationName,
                ApplicationVersion = CommonRuntimeData.ApplicationVersion,
                StartDateTime = CommonRuntimeData.StartDateTime
            };

            var json = CommonJsonSerializer.Serialize(response);
            context.Response.ContentType = MediaTypeNames.Application.Json;

            await context.Response.WriteAsync(json);
        }
    }
}
