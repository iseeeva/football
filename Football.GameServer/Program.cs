using Football.Common;
using Football.Database;
using Football.GameServer.Game;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Football.GameServer
{
    // ------------------- TODO LIST ---------------------
    // TODO1: Finish server browser.
    // TODO2: Track github issues.
    // ---------------------------------------------------

    internal class Program
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<Program>();
        private static readonly CancellationTokenSource _cts = new();
        private static GameMainServer? _gameServer;

        private static async Task Main(string[] args)
        {
            LogFactory.Configure();
            _log.Information("Logging started.");

            await ClearTransientStateAsync();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;

            _gameServer = new GameMainServer(3000, 100);
            _gameServer.Start();

            _log.Information("Server started. Press Ctrl+C to exit.");

            try
            {
                await Task.Delay(-1, _cts.Token);
            }
            catch (TaskCanceledException)
            {
                _log.Information("Shutdown requested.");
            }

            _gameServer.Stop();
        }

        private static async Task ClearTransientStateAsync()
        {
            try
            {
                using var db = new ServerBrowserDbContext();
                await db.BrowserSessions.ExecuteDeleteAsync();
                await db.ActiveMatches.ExecuteDeleteAsync();
                _log.Information("Temporary database context cleared.");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to clear temporary database context.");
            }
        }

        private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _log.Information("Ctrl+C detected. Shutting down...");
            e.Cancel = true;
            _cts.Cancel();
        }

        private static void OnProcessExit(object? sender, EventArgs e)
        {
            try
            {
                _gameServer?.Stop();
                ClearTransientStateAsync().GetAwaiter().GetResult();
                _log.Information("Exit cleanup complete.");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to clean up during exit.");
            }
        }
    }
}