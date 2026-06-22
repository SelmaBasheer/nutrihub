using Serilog;

namespace IdentityService.API.Extensions
{
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder AddSerilog(
            this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/nutrihub-identity-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                .CreateLogger();

            builder.Host.UseSerilog();
            return builder;
        }
    }

}
