// See https://aka.ms/new-console-template for more information

using Sobee.Common;

namespace Sobee
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //Console.WriteLine("Press Ctrl+C to stop the process.\n");

            Logging.Configure();
            Log.Information("Logging started.");

            Sobee.System.Common.Hub Hub = new(3000);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                //Hub.Stop();
                eventArgs.Cancel = true;
            };

            await Task.Delay(-1);
        }

        private static Serilog.ILogger Log
        {
            get { return Logging.Get<Program>(); }
        }
    }
}