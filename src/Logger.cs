using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Text.Json;

public static class Logger
{
    public static void Configure()
    {
        string template = "{Level:u1} > [{Prefix} {Additional}]: {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: template, theme: AnsiConsoleTheme.Code)
            .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day, outputTemplate: template)
            .Enrich.WithProperty("Prefix", "Default") // Default prefix
            .Enrich.WithProperty("Additional", "Default") // Default additional context
            .CreateLogger();
    }

    public static ILogger Get<T>(object? Additional = null)
    {
        return Log.ForContext<T>()
                  .ForContext("Prefix", typeof(T).FullName ?? "Unknown")
                  .ForContext("Additional", JsonSerializer.Serialize(Additional ?? new { }, new JsonSerializerOptions { }));
    }
}
