using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ms.Arch.Hw02.Api.Application.Infrastructure.Interfaces;
using Ms.Arch.Hw02.Api.Application.Services;
using Ms.Arch.Hw02.Api.CommonCore;
using Ms.Arch.Hw02.Api.CommonCore.CommonMiddleware;
using Ms.Arch.Hw02.Api.CommonCore.HealthCheck;
using Ms.Arch.Hw02.Api.CommonCore.Metrics;
using Ms.Arch.Hw02.Api.CommonCore.VersionsInfo;
using Ms.Arch.Hw02.Api.Persistence;

namespace Ms.Arch.Hw02.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCommonPrometheus();
            SetCommonServiceData(HostEnvironment);

            services.AddControllers();

            #region Configure

            services.Configure<LogRequestResponseSettings>(Configuration.GetSection(nameof(LogRequestResponseSettings)));
            services.AddSingleton<LogRequestResponseSettings>();

            PostgresDbConnect pgConnectSettings;
            var databaseUrlEnvironmentVariable = Environment.GetEnvironmentVariable(nameof(PostgresDbConnect));
            if (string.IsNullOrEmpty(databaseUrlEnvironmentVariable))
            {
                var postgresDbSection = Configuration.GetSection(nameof(PostgresDbConnect));
                services.Configure<PostgresDbConnect>(postgresDbSection);
                pgConnectSettings = postgresDbSection.Get<PostgresDbConnect>();
            }
            else
            {
                services.Configure<PostgresDbConnect>(x => x.ConnectionString = databaseUrlEnvironmentVariable);
                pgConnectSettings = new PostgresDbConnect {ConnectionString = databaseUrlEnvironmentVariable };
            }

            #endregion Configure

            #region ResolveServices

            services.AddSingleton<IUserService, UserService>();

            #endregion

            #region ResolveRepository

            services.AddSingleton<IUserRepository, UserRepository>();

            #endregion

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Arch.Hw02", Version = "v1"}); });

            services.AddHealthChecks()
                .AddNpgSql(pgConnectSettings.ConnectionString, "SELECT 1 FROM account.account",
                    tags: new[] { HealthCheckTags.Readiness }, name:"PostresReadiness");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arch.Hw02 v1"));

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();
            app.UseCommonHealthCheck();
            app.UseCommonVersion();
            app.UseCommonMetrics();
            app.UseMiddleware<CommonRequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync($"Hello from Oveyn with app: {env.ApplicationName}");
                    });

                endpoints.MapControllers();
            });
        }

        private static void SetCommonServiceData(IHostEnvironment context)
        {
            var applicationAssembly = Assembly.Load(context.ApplicationName);
            CommonRuntimeData.Initialize(applicationAssembly);
        }
    }
}