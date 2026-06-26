using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Football.Common
{
    public static class LogFactory
    {
        public static void Configure()
        {
            string template = "{Level:u1} > [{Prefix}] (from: {Component}): {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: template, theme: AnsiConsoleTheme.Code)
                .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day, outputTemplate: template)
                .Enrich.WithProperty("Prefix", "Default")
                .Enrich.WithProperty("Component", "Default")
                .CreateLogger();
        }

        public static ILogger GetContextForType<T>()
        {
            return GetContextForType(typeof(T));
        }

        public static ILogger GetContextForType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return Log.ForContext(type)
                      .ForContext("Prefix", type.Name)
                      .ForContext("Component", "Unknown");
        }

        public static ILogger GetContextForObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            Type type = obj.GetType();

            return Log.ForContext(type)
                      .ForContext("Prefix", type.Name)
                      .ForContext("Component", GetComponentIdentifier(obj));
        }

        private static string GetComponentIdentifier(object type)
        {
            if (type is not Component component)
                return "Unknown";

            return component.Id.ToString();
        }
    }
}