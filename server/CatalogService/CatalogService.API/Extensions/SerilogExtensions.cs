using Serilog;

namespace CatalogService.API.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration)
                  .Enrich.WithMachineName()
                  .Enrich.WithEnvironmentName()
                  .WriteTo.Console()
                  .WriteTo.File("logs/catalog-.log", rollingInterval: RollingInterval.Day));
        
        return builder;
    }
}
