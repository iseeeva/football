namespace Sobee.TestServer.Auth
{
    public class AuthCommunication : Communication
    {
        private bool _isDisposed;

        public AuthCommunication() : base()
        {
            //RegisterMessageEvent<Messages.PositioningCutscene>(new MessageDelegate(OnReceivedMessage<Messages.PositioningCutscene>));
            //RegisterMessageEvent<Messages.Auth.UserJoined>(new MessageDelegate(OnReceivedMessage<Messages.Auth.UserJoined>));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {

                }
            }

            base.Dispose(disposing);
        }
    }
}