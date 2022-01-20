using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using Ms.Arch.Hw01.Api.CommonCore.CommonMiddleware.Settings;

namespace Ms.Arch.Hw01.Api.CommonCore.CommonMiddleware
{
    public class CommonRequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CommonRequestResponseLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly LogRequestResponseSettings _logRequestResponseSettings;

        public CommonRequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<CommonRequestResponseLoggingMiddleware> logger,
            IOptions<LogRequestResponseSettings> logRequestResponseSettings)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _logRequestResponseSettings = logRequestResponseSettings.Value;
        }

        public async Task Invoke(
            HttpContext context)
        {
            if (_logRequestResponseSettings.LogRequest)
                await LogRequest(context.Request);

            if (_logRequestResponseSettings.LogResponse)
                await LogResponse(context);
            else
                await _next(context);
        }

        private async Task LogRequest(HttpRequest request)
        {
            request.EnableBuffering();

            _logger.LogInformation(
                $"Http Request Information:{Environment.NewLine}" +
                $"FullPath: \"{request.Method}\" {request.GetDisplayUrl()}{Environment.NewLine}" +
                $"Request Body: {await PrepareRequestMessage(request.Body)}{Environment.NewLine}");
            request.Body.Seek(0, SeekOrigin.Begin);
        }

        private async Task LogResponse(HttpContext context)
        {
            //Подменяем оригинальный стрим в боди на свой собственный что бы была возможность перечитывать его.
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();

            _logger.LogInformation(
                $"Http Response Information:{Environment.NewLine}" +
                $"FullPath: {context.Request.GetDisplayUrl()}{Environment.NewLine}" +
                $"StatusCode: {context.Response.StatusCode}({(HttpStatusCode) context.Response.StatusCode}). Duration: {sw.Elapsed.TotalMilliseconds} ms.{Environment.NewLine}" +
                $"Response Body: {await GetResponseBody(context)}{Environment.NewLine}");

            // Копируем данные в оригинальный стрим и возврашаем его обратно в тело
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }

        private static async Task<string> PrepareRequestMessage(Stream stream)
        {
            var input = await ReadRequestStreamAsync(stream);
            return input;
        }

        private static async Task<string> ReadRequestStreamAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, leaveOpen: true);
            var result = await reader.ReadToEndAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return result;
        }

        private async Task<string> GetResponseBody(HttpContext context)
        {
            return await PrepareResponseMessage(context.Response);
        }

        private async Task<string> PrepareResponseMessage(
            HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var output = await new StreamReader(response.Body).ReadToEndAsync();

            if (output.Length == 0)
                output = "<empty response>";

            return output;
        }
    }
}