//using InfluxDB.Client;
//using InfluxDB.Client.Api.Domain;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace KShop.Payments.WebApi.BackgroundServices
//{
//    /// <summary>
//    ///
//    /// </summary>
//    public class TestAlgorithmsBackgroundService : BackgroundService
//    {
//        private readonly ILogger<TestAlgorithmsBackgroundService> _logger;

//        public TestAlgorithmsBackgroundService(ILogger<TestAlgorithmsBackgroundService> logger)
//        {
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var influxToken = "-a23h6DztHAXkktsiz-O97-IeieBeZx5W6EQYWMWILnJdCSwa7XqQqelGoECpiQqLE7Kmmb1QsLDt-yimIl8OQ==";
//            var influxClient = InfluxDBClientFactory.Create("http://localhost:8086", influxToken.ToCharArray());
//            var influxBucket = "appmetrics";
//            var influxOrg = "kshop";

//            var stopwatch = new Stopwatch();

//            int round = 0;
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                stopwatch.Reset();
//                stopwatch.Start();
//                MD5_Hashing(10000, $"TestCpuLoaderBackgroundService {round}");
//                stopwatch.Stop();

//                var md5_ms = stopwatch.ElapsedMilliseconds;

//                stopwatch.Reset();
//                stopwatch.Start();
//                SHA256_Hashing(10000, $"TestCpuLoaderBackgroundService {round}");
//                stopwatch.Stop();

//                var sha_ms = stopwatch.ElapsedMilliseconds;

//                var data = $"performance,app=algo_hash_sha_md5 md5_ms={md5_ms},sha_ms={sha_ms} {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

//                using (var writeApi = influxClient.GetWriteApi())
//                {
//                    _logger.LogInformation("TELEMETRY: {TelemetryData}", data);
//                    writeApi.WriteRecord(influxBucket, influxOrg, WritePrecision.S, data);
//                }

//                round++;
//                await Task.Delay(TimeSpan.FromMilliseconds(5000));
//            }

            
//        }

//        private void MD5_Hashing(int count, string raw_str)
//        {
//            var md5 = MD5.Create();

//            for (int i = 0; i < count; i++)
//            {
//                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{raw_str} - {i}"));
//            }
//        }

//        private void SHA256_Hashing(int count, string raw_str)
//        {
//            var sha256 = SHA256.Create();

//            for (int i = 0; i < count; i++)
//            {
//                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{raw_str} - {i}"));
//            }
//        }
//    }
//}
