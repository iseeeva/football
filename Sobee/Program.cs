using Serilog;
using Sobee.Common;

namespace Sobee
{
    internal class Program
    {
        private static readonly ILogger _log = Logging.Get<Program>();

        private static async Task Main(string[] args)
        {
            //Console.WriteLine("Press Ctrl+C to stop the process.\n");

            Logging.Configure();
            _log.Information("Logging started.");

            TestServer.Hub Hub = new(3000);
            Hub.Start();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                //Hub.Stop();
                eventArgs.Cancel = true;
            };

            await Task.Delay(-1);
        }
    }
}