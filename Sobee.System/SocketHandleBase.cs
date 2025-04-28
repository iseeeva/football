using System.Net.Sockets;
using Serilog;
using Sobee.Common;
using Sobee.Messaging;

namespace Sobee.System
{
    public class SocketHandleBase : SocketMessageHandle
    {
        private readonly ILogger log = Logging.Get<SocketHandleBase>();

        public Guid Id { get; private set; }

        public event EventHandler? onDisconnect;

        public SocketHandleBase(Guid id, Socket socket) : base(socket)
        {
            Id = id;
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
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                clientSocket.Close();
                clientSocket.Dispose();
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
                log.Information("disposed.");
                GC.SuppressFinalize(this);
                base.Dispose();
            }
        }
    }
}
