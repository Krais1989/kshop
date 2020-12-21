using KShop.Catalogues.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketCLI
{
    class Program
    {
        public static ILoggerFactory LoggerFactory;
        public static IConfigurationRoot Configuration;
        public static IServiceProvider Services;
        public

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ConfigureServices();


            var cancelToken = new CancellationTokenSource().Token;

            Log.Information($"MARKET CLI ENTER COMMANDS");

            string cmdLine;
            while ((cmdLine = Console.ReadLine()) != "exit")
            {
                IBaseCommandAsync cmd = null;
                switch (cmdLine)
                {
                    case "create":
                        cmd = Services.GetService<CatalogueCreateCommand>();
                        break;
                    case "delete":
                        cmd = Services.GetService<CatalogueDeleteCommand>();
                        break;
                    case "truncate":
                        cmd = Services.GetService<CatalogueTruncateCommand>();
                        break;
                    case "drop tables":
                        cmd = Services.GetService<CatalogueDropTableCommand>();
                        break;

                }

                if (cmd == null)
                    Log.Error($"Undefined command {cmdLine}");
                else
                {
                    await cmd.ExecuteAsync(cancelToken);
                    Log.Information($">>> COMPLETE \"{cmdLine}\" <<<");
                }
            }
        }

        static void ConfigureServices()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            services.AddDbContext<CatalogueContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            });

            services.AddTransient<CatalogueCreateCommand>();
            services.AddTransient<CatalogueDeleteCommand>();
            services.AddTransient<CatalogueDropTableCommand>();
            services.AddTransient<CatalogueTruncateCommand>();

            services.AddLogging(x => x.AddSerilog());

            Services = services.BuildServiceProvider();
        }
    }
}
