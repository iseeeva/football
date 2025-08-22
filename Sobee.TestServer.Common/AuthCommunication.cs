using Sobee.Messaging;
using Sobee.Network;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Common
{
    public class AuthCommunication : Communication
    {
        private bool _isDisposed;
        private EventHandler<MessageEventArgs> _positioningCutsceneEventHandler;
        private EventHandler<MessageEventArgs> _playerInformationEventHandler;
        private EventHandler<MessageEventArgs> _playerJoinedEventHandler;

        public AuthCommunication()
        {
            Initialize();
            base.RegisterMessageEvent(typeof(PositioningCutscene), new MessageDelegate(this.OnPositioningCutscene));
            base.RegisterMessageEvent(typeof(PlayerInformation), new MessageDelegate(this.OnPlayerInformation));
            base.RegisterMessageEvent(typeof(PlayerJoined), new MessageDelegate(this.OnPlayerJoined));
        }

        private void Initialize()
        {

        }

        #region PositioningCutscene

        public void SubscribePositioningCutscene(EventHandler<MessageEventArgs> handler)
        {
            this._positioningCutsceneEventHandler = (EventHandler<MessageEventArgs>)Delegate.Combine(this._positioningCutsceneEventHandler, handler);
        }

        public void UnsubscribePositioningCutscene(EventHandler<MessageEventArgs> handler)
        {
            this._positioningCutsceneEventHandler = (EventHandler<MessageEventArgs>)Delegate.Remove(this._positioningCutsceneEventHandler, handler);
        }

        protected virtual void OnPositioningCutscene(Session sessionConnection, Message message)
        {
            if (this._positioningCutsceneEventHandler != null)
            {
                this._positioningCutsceneEventHandler(this, new MessageEventArgs(sessionConnection, message));
            }
        }

        #endregion

        #region PlayerInformation

        public void SubscribePlayerInformation(EventHandler<MessageEventArgs> handler)
        {
            this._playerInformationEventHandler = (EventHandler<MessageEventArgs>)Delegate.Combine(this._playerInformationEventHandler, handler);
        }

        public void UnsubscribePlayerInformation(EventHandler<MessageEventArgs> handler)
        {
            this._playerInformationEventHandler = (EventHandler<MessageEventArgs>)Delegate.Remove(this._playerInformationEventHandler, handler);
        }

        protected virtual void OnPlayerInformation(Session sessionConnection, Message message)
        {
            if (this._playerInformationEventHandler != null)
            {
                this._playerInformationEventHandler(this, new MessageEventArgs(sessionConnection, message));
            }
        }

        #endregion

        #region PlayerJoined

        public void SubscribePlayerJoined(EventHandler<MessageEventArgs> handler)
        {
            this._playerJoinedEventHandler = (EventHandler<MessageEventArgs>)Delegate.Combine(this._playerJoinedEventHandler, handler);
        }

        public void UnsubscribePlayerJoined(EventHandler<MessageEventArgs> handler)
        {
            this._playerJoinedEventHandler = (EventHandler<MessageEventArgs>)Delegate.Remove(this._playerJoinedEventHandler, handler);
        }

        protected virtual void OnPlayerJoined(Session sessionConnection, Message message)
        {
            if (this._playerJoinedEventHandler != null)
            {
                this._playerJoinedEventHandler(this, new MessageEventArgs(sessionConnection, message));
            }
        }

        #endregion

        public override Session CreateSession(SocketWrapper socketConnection)
        {
            return new Session(socketConnection, this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    this._positioningCutsceneEventHandler = null;
                    this._playerInformationEventHandler = null;
                    this._playerJoinedEventHandler = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}