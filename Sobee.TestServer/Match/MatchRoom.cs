using Sobee.Network;
using Sobee.TestServer.MatchComponents;
using Sobee.TestServer.MatchEvents;
using Sobee.TestServer.Messages;
using Sobee.TestServer.Messages.Ball;
using Sobee.TestServer.Messages.Chat;
using Sobee.TestServer.Messages.Match;
using Sobee.TestServer.Messages.Player;

namespace Sobee.TestServer.Match
{
    public class MatchRoom : Room<MatchRoom, MatchPlayer>
    {
        public readonly MatchInformationMessage MatchInformation = new();
        public MatchPlayerManager<MatchRoom, MatchPlayer> Players => (MatchPlayerManager<MatchRoom, MatchPlayer>)_sessions;
        public RoomCommunication Communication => _communication;

        public static readonly TimeSpan MAX_IDLE_TIME = TimeSpan.FromMinutes(1);
        public static readonly int MAX_PLAYER = ScenarioInfo.MAX_TEAM_SIZE * 2;

        #region Constructor
        public MatchRoom()
        {
            // === Events ===
            Players.PlayerJoinEvent += MatchServerPlayerEvent.PlayerJoinReceived;
            Players.PlayerLeaveEvent += MatchServerPlayerEvent.PlayerLeaveReceived;

            // === Components ===
            AddComponent(new MatchMovementComponent());
            AddComponent(new MatchBallComponent());
            AddComponent(new MatchTimeComponent());

            // === Communication ===
            Communication.CommunicationType = SessionType.Game;

            // === Player Messages ===
            Communication.RegisterMessage<PlayerHeartbeatRxMessage>();
            Communication.RegisterMessage<PlayerMoveKeyUpRxMessage>();
            Communication.RegisterMessage<PlayerMoveKeyDownRxMessage>();
            Communication.RegisterMessage<PlayerMatchStateAlertRxMessage>();
            Communication.RegisterMessage<ChatPlayerTextRxMessage>();

            // === Match Messages ===
            Communication.RegisterMessage<BallPositioningRxMessage>();
            Communication.AddGlobalMessageHandler<BallPositioningRxMessage>(MatchClientBallEvent.BallPositioningReceived);
            Communication.RegisterMessage<BallTackleRxMessage>();
            Communication.AddGlobalMessageHandler<BallTackleRxMessage>(MatchClientBallEvent.BallTackleReceived);
            Communication.RegisterMessage<BallInterceptRxMessage>();
            Communication.AddGlobalMessageHandler<BallInterceptRxMessage>(MatchClientBallEvent.BallInterceptReceived);
            Communication.RegisterMessage<BallPassNormalRxMessage>();
            Communication.AddGlobalMessageHandler<BallPassNormalRxMessage>(MatchClientBallEvent.BallPassNormalReceived);
            Communication.RegisterMessage<BallPassThroughRxMessage>();
            Communication.AddGlobalMessageHandler<BallPassThroughRxMessage>(MatchClientBallEvent.BallPassThroughReceived);
            Communication.RegisterMessage<BallPassLongRxMessage>();
            Communication.AddGlobalMessageHandler<BallPassLongRxMessage>(MatchClientBallEvent.BallPassLongReceived);
            Communication.RegisterMessage<BallShootRxMessage>();
            Communication.AddGlobalMessageHandler<BallShootRxMessage>(MatchClientBallEvent.BallShootReceived);
        }
        #endregion

        #region Lifecycle
        //protected override void OnStart()
        //{
        //    base.OnStart();
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //}

        //protected override void OnUpdate(double delta)
        //{
        //    base.OnUpdate(delta);
        //}
        #endregion

        #region Session Manager
        protected override SessionManager<MatchRoom, MatchPlayer> CreateSessionManager()
               => new MatchPlayerManager<MatchRoom, MatchPlayer>();
        #endregion

        #region Dispose
        //protected override void OnDispose()
        //{
        //    base.OnDispose();
        //}
        #endregion
    }
}