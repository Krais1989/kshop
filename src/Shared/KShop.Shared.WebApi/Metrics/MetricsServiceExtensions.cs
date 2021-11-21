//using App.Metrics;
//using App.Metrics.Formatters.Ascii;
//using App.Metrics.Formatters.InfluxDB;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;

//namespace KShop.Shared.WebApi
//{
//    public static class MetricsServiceExtensions
//    {
//        public class MetricsOptions
//        {
//            public string DefaultContextLabel { get; set; }
//            public InfluxReportOptions InfluxOptions { get; set; }
//        }

//        public class InfluxReportOptions
//        {
//            public class CHttpPolicy
//            {
//                public string BackoffPeriod { get; set; }
//                public int FailuresBeforeBackoff { get; set; }
//                public string Timeout { get; set; }
//                public TimeSpan TimeoutTimespan => TimeSpan.Parse(Timeout);
//                public TimeSpan BackoffPeriodTimespan => TimeSpan.Parse(BackoffPeriod);
//            }

//            public string BaseUri { get; set; }
//            public string Database { get; set; }
//            public string UserName { get; set; }
//            public string Password { get; set; }

//            public string FlushInterval { get; set; }
//            public TimeSpan FlushIntervalTimespan => TimeSpan.Parse(FlushInterval);

//            public CHttpPolicy HttpPolicy { get; set; }
//        }

//        public static IServiceCollection AddKShopMetrics(this IServiceCollection services, IConfiguration config)
//        {
//            var trace_options = new MetricsOptions();
//            config.GetSection("MetricsOptions").Bind(trace_options);

//            //var filter = new MetricsFilter().WhereType(MetricType.Counter| MetricType.Timer);
//            var builder = AppMetrics.CreateDefaultBuilder();
//            //builder.Configuration.ReadFrom(Configuration);

//            var metrics = builder
//                .OutputMetrics.AsInfluxDbLineProtocol()
//                .OutputEnvInfo.AsPlainText()
//                .Configuration.Configure(opt =>
//                {
//                    //opt.DefaultContextLabel = "KShop";
//                    opt.Enabled = true;
//                    opt.ReportingEnabled = true;
//                    opt.DefaultContextLabel = trace_options.DefaultContextLabel;
//                    //opt.GlobalTags.Add("mytag", "mytagvalue");
//                })
//                .Report.ToInfluxDb(influx =>
//                {
//                    var influx_options = trace_options.InfluxOptions;
//                    influx.InfluxDb.BaseUri = new Uri(influx_options.BaseUri);
//                    influx.InfluxDb.Database = influx_options.Database;
//                    //influx.InfluxDb.Consistenency = "consistency";
//                    influx.InfluxDb.UserName = influx_options.UserName;
//                    influx.InfluxDb.Password = influx_options.Password;
//                    //influx.InfluxDb.RetentionPolicy = "rp";
//                    influx.InfluxDb.CreateDataBaseIfNotExists = true;

//                    var http_policy = influx_options.HttpPolicy;
//                    influx.HttpPolicy.BackoffPeriod = http_policy.BackoffPeriodTimespan;
//                    influx.HttpPolicy.FailuresBeforeBackoff = http_policy.FailuresBeforeBackoff;
//                    influx.HttpPolicy.Timeout = http_policy.TimeoutTimespan;

//                    influx.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
//                    influx.FlushInterval = influx_options.FlushIntervalTimespan;
//                })
//                .Build();

//            services.AddAppMetricsHealthPublishing();
//            services.AddAppMetricsGcEventsMetricsCollector();
//            services.AddAppMetricsSystemMetricsCollector();

//            services.AddMetrics(metrics);
//            services.AddMetricsTrackingMiddleware();
//            services.AddMetricsEndpoints(opt =>
//            {
//                // site/metrics
//                opt.MetricsEndpointEnabled = true;
//                opt.MetricsEndpointOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();

//                // site/metrics-text
//                opt.MetricsTextEndpointEnabled = true;
//                opt.MetricsTextEndpointOutputFormatter = new MetricsTextOutputFormatter();

//                // site/env
//                opt.EnvironmentInfoEndpointEnabled = true;
//                opt.EnvInfoEndpointOutputFormatter = new EnvInfoTextOutputFormatter();
//            });

//            services.AddMetricsReportingHostedService();

//            return services;
//        }
//    }
//}
