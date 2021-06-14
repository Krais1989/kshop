using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Carts.WebApi
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
                .ConfigureServices((ctx, services) => {

                })
                .ConfigureAppConfiguration((host, cfg) => {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    cfg.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false);
                    cfg.AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true);
                })
                .UseSerilog((context, log) => { log.ReadFrom.Configuration(context.Configuration); });
    }
}
