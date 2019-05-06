using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Prestige.Kernel.Common.Constants;
using Prestige.Kernel.Common.Extensions;
using Prestige.Kernel.Common.Models.Logging;

using Serilog;
using Serilog.Sinks.Elasticsearch;

using System;

namespace Prestige.Kernel.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHostBuilder)
            => webHostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                IConfiguration configuration = context.Configuration;
                var appNameProperty = GlobalConstants.LoggingApplicationNameProperty;

                loggerConfiguration.Enrich.FromLogContext()
                    .Enrich.WithProperty(appNameProperty, configuration[appNameProperty]);
                Configure(loggerConfiguration, configuration.GetOptions<ElkOptions>(GlobalConstants.ElkOptionsSectionName));
            });

        private static void Configure(LoggerConfiguration loggerConfiguration, ElkOptions elkOptions)
        {
            if (elkOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elkOptions.Uri))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = GlobalConstants.LogstashIndexFormat
                });
            }
        }
    }
}
