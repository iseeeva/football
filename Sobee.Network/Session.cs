using System.Net.Sockets;
using Serilog;
using Sobee.Common;

namespace Sobee.Network
{
    public class Session : Component
    {
        private static readonly ILogger _log = Logging.Get<Session>();
        private bool _isDisposed;

        public readonly Socket Socket;
        public readonly SessionType SessionType;

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
                    this.Dispose();
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public Session(Socket socket, SessionType sessionType)
        {
            this.Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            this.SessionType = sessionType;

            try
            {
                _log.Debug("{id} initialized.", this.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public override Task Update(double delta)
        {
            return Task.CompletedTask;
        }

        public virtual void Disconnect()
        {
            if (!IsConnected) return;

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
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    _log.Debug("{id} disposing.", this.Id);

                    Disconnect();
                    Socket.Dispose();

                    _log.Debug("{id} disposed.", this.Id);
                }
            }

            base.Dispose(disposing);
        }
    }
}
