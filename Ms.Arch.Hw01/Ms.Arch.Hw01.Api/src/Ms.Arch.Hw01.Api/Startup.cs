using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ms.Arch.Hw01.Api.CommonCore;
using Ms.Arch.Hw01.Api.CommonCore.CommonMiddleware;
using Ms.Arch.Hw01.Api.CommonCore.CommonMiddleware.Settings;
using Ms.Arch.Hw01.Api.CommonCore.HealthCheck;
using Ms.Arch.Hw01.Api.CommonCore.VersionsInfo;

namespace Ms.Arch.Hw01.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            SetCommonServiceData(Environment);
            services.AddHealthChecks();

            #region Configure

            services.Configure<LogRequestResponseSettings>(
                Configuration.GetSection(nameof(LogRequestResponseSettings)));
            services.AddSingleton<LogRequestResponseSettings>();

            #endregion Configure
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCommonHealthCheck();
            app.UseCommonVersion();
            app.UseMiddleware<CommonRequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync($"Hello from Oveyn with app: {env.ApplicationName}");
                    });
            });
        }

        private static void SetCommonServiceData(IHostEnvironment context)
        {
            var applicationAssembly = Assembly.Load(context.ApplicationName);
            CommonRuntimeData.Initialize(applicationAssembly);
        }
    }
}