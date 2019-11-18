using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace JOS.Multiple.HostedServices.Features.Shared.Logging
{
    public static class SerilogConfigurator
    {
        private const string OutputFormat = "[{Timestamp:HH:mm:ss} {Level:u3}][{CorrelationId}] {Message}{NewLine}{Exception}";
        public static void ConfigureLoggingFeature(
            this ILoggingBuilder loggingBuilder,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration,
            IServiceCollection services)
        {
            loggingBuilder.AddSerilog();
            var loggerConfiguration = new LoggerConfiguration();
            
            ConfigureLogLevel(loggerConfiguration, hostEnvironment, configuration);
            ConfigureConsoleLogging(loggerConfiguration, hostEnvironment, configuration);
            ConfigureFileLogging(loggerConfiguration, hostEnvironment, configuration);
            ConfigureElasticSearchLogging(loggerConfiguration, hostEnvironment, configuration);

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private static void ConfigureLogLevel(
            LoggerConfiguration loggerConfiguration,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            var minimumLevelConfig = configuration.GetValue<string>("Logging:MinimumLevel:Default");
            var microsoftLevelConfig = configuration.GetValue<string>("Logging:MinimumLevel:Microsoft");
            var systemLevelConfig = configuration.GetValue<string>("Logging:MinimumLevel:System");
            
            Enum.TryParse(minimumLevelConfig ?? (hostEnvironment.IsDevelopment() ? "Debug" : "Information"), out LogEventLevel minimumLevel);
            Enum.TryParse(microsoftLevelConfig ?? (hostEnvironment.IsDevelopment() ? "Information" : "Warning"), out LogEventLevel microsoftLevel);
            Enum.TryParse(systemLevelConfig ?? (hostEnvironment.IsDevelopment() ? "Information" : "Warning"), out LogEventLevel systemLevel);

            loggerConfiguration.MinimumLevel.Is(minimumLevel)
                .MinimumLevel.Override("Microsoft", microsoftLevel)
                .MinimumLevel.Override("System", systemLevel);
        }

        private static void ConfigureConsoleLogging(
            LoggerConfiguration loggerConfiguration,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            var shouldLogToConsoleConfig = configuration.GetValue<bool?>("Logging:Output:Console:Enabled") ?? hostEnvironment.IsDevelopment();
            if (shouldLogToConsoleConfig)
            {
                loggerConfiguration.WriteTo.Async(a => a.Console(outputTemplate: OutputFormat));
            }
        }

        private static void ConfigureFileLogging(
            LoggerConfiguration loggerConfiguration,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            var shouldLogToFile = configuration.GetValue<bool?>("Logging:Output:File:Enabled") ?? hostEnvironment.IsDevelopment();
            if (shouldLogToFile)
            {
                var logDirectory = configuration.GetValue<string>("Logging:Output:File:Directory") ?? $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}logs";
                var fileSizeLimit = configuration.GetValue<int?>("Logging:Output:File:FileSizeLimitBytes") ?? 100000000;
                var flushInterval = configuration.GetValue<int?>("Logging:Output:File:FlushIntervalSeconds") ?? TimeSpan.FromSeconds(10).TotalSeconds;

                loggerConfiguration.WriteTo.Async(x => x.File(
                    outputTemplate: OutputFormat,
                    path: $"{logDirectory}JOS.Multiple.HostedServices..txt",
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: fileSizeLimit,
                    rollingInterval: RollingInterval.Day,
                    buffered: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(flushInterval)
                ));
            }
        }

        private static void ConfigureElasticSearchLogging(
            LoggerConfiguration loggerConfiguration,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            var shouldLogToElasticSearch = configuration.GetValue<bool?>("Logging:Output:ElasticSearch:Enabled") ?? !hostEnvironment.IsDevelopment();

            if (shouldLogToElasticSearch)
            {
                var nodeUrls = configuration.GetValue<string[]>("Logging:Output:ElasticSearch:Nodes") ?? new []{"http://localhost:9200"};
                var logDirectory = configuration.GetValue<string>("Logging:Output:File:Directory") ?? $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}logs";
                var fileSizeLimit = configuration.GetValue<int?>("Logging:Output:File:FileSizeLimitBytes") ?? 100000000;
                var options = new ElasticsearchSinkOptions(nodeUrls.Select(x => new Uri(x, UriKind.Absolute)))
                {
                    EmitEventFailure = EmitEventFailureHandling.WriteToFailureSink,
#pragma warning disable 618
                    FailureSink = new FileSink(
#pragma warning restore 618
                        $"{logDirectory}.failure.JOS.Multiple.HostedServices..txt",
                        new MessageTemplateTextFormatter(OutputFormat, CultureInfo.InvariantCulture),
                        fileSizeLimitBytes: fileSizeLimit),
                    QueueSizeLimit = 1000000
                };

                loggerConfiguration.WriteTo.Elasticsearch(options);
            }
        }

    }
}
