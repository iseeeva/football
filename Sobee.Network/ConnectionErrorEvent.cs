using System.Net.Sockets;

namespace Sobee.Network
{
    public class ConnectionErrorEvent : EventArgs
    {
        public readonly SocketWrapper? Socket;
        public readonly ConnectionError ConnectionError;
        public readonly SocketError SocketError;

        public ConnectionErrorEvent(SocketError socketError_1)
        {
            ConnectionError = ConnectionError.SocketError;
            SocketError = socketError_1;
        }

        public ConnectionErrorEvent(ConnectionError connectionError_1)
        {
            ConnectionError = connectionError_1;
            SocketError = SocketError.Success;
        }

        public ConnectionErrorEvent(SocketWrapper gclass297_1)
        {
            Socket = gclass297_1;
        }

        public override string ToString()
        {
            if (ConnectionError == ConnectionError.SocketError)
            {
                return "SocketError: " + SocketError.ToString();
            }
            return "ConnectionError: " + ConnectionError.ToString();
        }
    }
}