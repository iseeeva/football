// See https://aka.ms/new-console-template for more information

using Sobee.Network.Socket;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //Console.WriteLine("Press Ctrl+C to stop the process.\n");

        Logger.Configure();
        Log.Information("Logging started.");

        Sobee.Network.Socket.Hub Hub = new(3000);

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Hub.Stop();
            eventArgs.Cancel = true;
        };

        await Task.Delay(-1);
    }

    private static Serilog.ILogger Log
    {
        get { return Logger.Get<Program>(); }
    }
}