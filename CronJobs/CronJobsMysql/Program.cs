using System.IO;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CronJobsMysql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(SettingManager.GetValue("HostUrl"));
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                        .Enrich.FromLogContext()
                        .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Hour);
                });
    }
}
