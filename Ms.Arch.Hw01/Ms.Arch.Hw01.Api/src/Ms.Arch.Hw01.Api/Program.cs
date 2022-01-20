using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ms.Arch.Hw01.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Application invoked with arguments: '{String.Join(" ", args)}'");

            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                    });
                });
            return host;
        }
    }
}
