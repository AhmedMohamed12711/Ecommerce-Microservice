

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Common.Logging;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (Context, LoggerConfiguration) =>
    {
        var env= Context.HostingEnvironment;
        LoggerConfiguration.MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", env.ApplicationName)
        .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
        .Enrich.WithExceptionDetails()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
        .WriteTo.Console();

        if (Context.HostingEnvironment.IsDevelopment())
        {
            LoggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
            LoggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
            LoggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
            LoggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
        }

        //Configure Elastic Search
        var ElasticUrl = Context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
        if (!string.IsNullOrEmpty(ElasticUrl)) 
        {
            LoggerConfiguration.WriteTo.Elasticsearch(
                new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(ElasticUrl))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                    IndexFormat="newecommerce-logs-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Debug
                });
        
        }
    };
}
