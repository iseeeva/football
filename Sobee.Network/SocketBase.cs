using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.Network
{
    public class SocketBase : Component
    {
        private readonly ILogger log = Logging.Get<SocketQueueHandle>();

        public Socket Socket { get; protected set; }

        public bool IsConnected
        {
            get
            {
                try
                {
                    return !(Socket.Poll(1, SelectMode.SelectRead) && Socket.Available == 0);
                }
                catch (SocketException)
                {
                    return false;
                }
            }
        }

        public SocketBase(Socket socket)
        {
            this.Socket = socket ?? throw new ArgumentNullException(nameof(socket));

            try
            {
                log.Debug("{id} initialized.", Socket.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public override async Task Update()
        {

        }

        public override void Dispose()
        {
            log.Debug("{id} disposing.", Socket.RemoteEndPoint);

            Socket?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
