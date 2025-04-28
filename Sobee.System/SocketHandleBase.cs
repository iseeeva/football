using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System
{
    public class SocketHandleBase : SocketMessageHandle
    {
        private readonly ILogger log = Logging.Get<SocketHandleBase>();

        public event EventHandler? onDisconnect;

        public SocketHandleBase(Socket socket) : base(socket)
        {

        }

        public override Task Update()
        {
            return base.Update();
        }

        protected virtual void Disconnect()
        {
            if (!IsConnected()) return;

            try
            {
                Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                Socket.Close();
                Socket.Dispose();
            }

            onDisconnect?.Invoke(this, EventArgs.Empty);
        }

        public override void Dispose()
        {
            try
            {
                Disconnect();
            }
            finally
            {
                log.Information("{id} disposing.", Socket.RemoteEndPoint);
                GC.SuppressFinalize(this);
                base.Dispose();
            }
        }
    }
}
