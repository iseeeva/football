using Serilog;
using Sobee.Common;

namespace Sobee
{
    internal class Sobee
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<Sobee>();

        private static async Task Main(string[] args)
        {
            LogFactory.Configure();
            _log.Information("Logging started.");

            TestServer.TestServer TestServer = new(3000, 100);
            TestServer.Start();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                //Hub.Stop();
                eventArgs.Cancel = true;
            };

            await Task.Delay(-1);
        }
    }
}