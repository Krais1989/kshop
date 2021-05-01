//using App.Metrics;
//using InfluxDB.Client;
//using InfluxDB.Client.Api.Domain;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace KShop.Payments.WebApi.BackgroundServices
//{
//    public class TelemetrySenderBackgroundService : BackgroundService
//    {
//        private readonly ILogger<TelemetrySenderBackgroundService> _logger;
//        private readonly IMetricsRoot _metrics;

//        public TelemetrySenderBackgroundService(ILogger<TelemetrySenderBackgroundService> logger, IMetricsRoot metrics)
//        {
//            _logger = logger;
//            _metrics = metrics;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var token = "-a23h6DztHAXkktsiz-O97-IeieBeZx5W6EQYWMWILnJdCSwa7XqQqelGoECpiQqLE7Kmmb1QsLDt-yimIl8OQ==";

//            var influxClient = InfluxDBClientFactory.Create("http://localhost:8086", token.ToCharArray());
//            var influxBucket = "appmetrics";
//            var influxOrg = "kshop";

//            var nfi = new NumberFormatInfo();
//            nfi.NumberDecimalSeparator = ".";

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                var proc = Process.GetCurrentProcess();
//                var memKb = proc.WorkingSet64 / 1048576;
//                var cpuLoad = await GetCpuUsageForProcess(TimeSpan.FromMilliseconds(1000));
//                var data = $"performance,app=payments mem={memKb},cpu={cpuLoad.ToString(nfi)} {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

//                using (var writeApi = influxClient.GetWriteApi())
//                {
//                    _logger.LogInformation("TELEMETRY: {TelemetryData}", data);
//                    writeApi.WriteRecord(influxBucket, influxOrg, WritePrecision.S, data);
//                }

//                var snapshotValues = _metrics.Snapshot.Get();

//                //await Task.Delay(TimeSpan.FromSeconds(2));
//            }
//        }

//        protected async Task<double> GetCPULoadAsync(TimeSpan period)
//        {
//            var proc = Process.GetCurrentProcess();

//            var startTime = DateTime.UtcNow;
//            var startCpuUsage = proc.TotalProcessorTime;
//            await Task.Delay(period);
//            var endTime = DateTime.UtcNow;
//            var endCpuUsage = proc.TotalProcessorTime;

//            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
//            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

//            var used = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

//            return used * 100;
//        }

//        private async Task<double> GetCpuUsageForProcess(TimeSpan period)
//        {
//            var startTime = DateTime.UtcNow;
//            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
//            await Task.Delay(period);

//            var endTime = DateTime.UtcNow;
//            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
//            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
//            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
//            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
//            return cpuUsageTotal * 100;
//        }

//    }

//}
