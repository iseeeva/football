using Serilog;
using Sobee.Common;

namespace Sobee
{
    internal class Program
    {
        private static readonly ILogger _log = Logging.Get<Program>();

        private static async Task Main(string[] args)
        {
            // PLAN: Planlanan islem semasi asagidaki gibidir.
            // ====================================================

            // AuthRoom: Auth bekleyenlerin bulundugu oda
            // => Ne yapacak?: Gelen kullanicinin auth bilgisini kontrol edip dogruysa MatchRoom'a yonlendirecek.
            // ==================================
            // Eventler:
            // => AuthInformationReceived: Gelen auth bilgisini kontrol eder. eger dogruysa istenen MatchRoom'a gonderir.
            // ==================================

            // MatchRoom: Mac odasi
            // => Ne yapacak?: Gelen kullaniciyi kontrol edip maca baglayacak.
            // ==================================

            Logging.Configure();
            _log.Information("Logging started.");

            TestServer.Hub Hub = new(3000);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                //Hub.Stop();
                eventArgs.Cancel = true;
            };

            await Task.Delay(-1);
        }
    }
}