namespace CRUD_API.Logging;
using Serilog;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        //1 . Configuration de Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Filter.ByExcluding(logEvent => 
                logEvent.Properties.ContainsKey("RequestPath") && 
                logEvent.Properties["RequestPath"].ToString().Contains("/swagger"))
            .WriteTo.Console(
                 theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code,
                 outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // 2 . Import : On lie Serilog à l'infrastructure de logging d'ASP.NET Core

        builder.Host.UseSerilog();

        Log.Information("Serilog has been configured successfully.");

    }
}
