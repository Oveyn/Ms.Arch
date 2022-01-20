using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ms.Arch.Hw02.Api.Persistence.Migrations.FluentMigrations;

namespace Ms.Arch.Hw02.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Application invoked with arguments: '{String.Join(" ", args)}'");

            var host = CreateHostBuilder(args)
                .Build();

            host.Services.Migrate();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(logging => { logging.AddConsole(); });
                }).ConfigureServices(services => { services.ConfigureFluentMigrator(); });
            return host;
        }

        public static IServiceCollection ConfigureFluentMigrator(this IServiceCollection services)
        {
            services.AddSingleton<MigrationsService>();

            services
                .AddOptions<MigrationsOptions>()
                .Configure<IHostEnvironment>((options, hostEnvironment) =>
                {
                    options.Enabled = true;
                    options.Assembly = Assembly.Load(hostEnvironment.ApplicationName);
                });

            services
                .AddOptions<MigrationsOptions>()
                .PostConfigure<IOptions<PostgresDbConnect>>((options, dataAccessOptions) =>
                {
                    if (options.ConnectionString == null)
                        options.ConnectionString = dataAccessOptions.Value.ConnectionString;
                });

            return services;
        }
    }
}