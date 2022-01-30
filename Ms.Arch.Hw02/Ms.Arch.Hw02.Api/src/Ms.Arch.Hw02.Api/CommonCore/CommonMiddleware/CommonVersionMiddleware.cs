using System.Net.Mime;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Ms.Arch.Hw02.Api.CommonCore.VersionsInfo;

namespace Ms.Arch.Hw02.Api.CommonCore.CommonMiddleware
{
    internal class CommonVersionMiddleware
    {
        public CommonVersionMiddleware(RequestDelegate next)
        {
            //ignore
        }

        [UsedImplicitly]
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
