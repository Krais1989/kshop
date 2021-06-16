using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Shipments.WebApi
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
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((ctx, services) =>
                {

                })
                .ConfigureAppConfiguration((host, cfg) =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    cfg.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false);
                    cfg.AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true);
                })
                .UseSerilog((context, log) => { log.ReadFrom.Configuration(context.Configuration); })
                .UseMetrics()
                .UseMetricsEndpoints()
                .UseMetricsWebTracking()
                .ConfigureAppMetricsHostingConfiguration(opt =>
                {
                    // AllEndpointsPort              Allows a port to be specified on which the configured endpoints will be available.This value will override any other endpoint port configuration.
                    // EnvironmentInfoEndpoint       The path to use for the environment info endpoint, the defaults is /env.
                    // EnvironmentInfoEndpointPort   The port to use for the environment info endpoint, if not specified uses your application’s default port.
                    // MetricsEndpoint               The path to use for the metrics endpoint, the defaults is /metrics.
                    // MetricsEndpointPort           The port to use for the metrics endpoint, if not specified uses your application’s default port.
                    // MetricsTextEndpoint           The path to use for the metrics text endpoint, the defaults is /metrics-text.
                    // MetricsTextEndpointPort       The port to use for the metrics text endpoint, if not specified uses your application’s default port.
                });
    }
}
