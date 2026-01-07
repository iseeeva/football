using System.Numerics;
using Sobee.Common;
using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchBall : MatchComponent
    {
        private readonly Serilog.ILogger _log = Logging.Get<MatchBall>();
        private bool _isDisposed;

        /// <summary>Minimum speed before stopping the ball movement</summary>
        public float Epsilon { get; set; } = 10f;
        /// <summary>Gravity affecting the ball's vertical movement</summary>
        public float Gravity { get; set; } = -9.81f * 100f * 2.5f;
        /// <summary>Reduction factor for ball speed over time</summary>
        public float Reduction { get; set; } = 0.985f;

        /// <summary>Radius for ball collision detection</summary>
        public double BallCollisionRadius { get; set; } = 15;
        /// <summary>Maximum speed of the ball</summary>
        public double BallMaxSpeed { get; set; } = 2000;
        /// <summary>Boundary for the ball (Z axis is ground level)</summary>
        public Vector3 BallBoundary { get; set; } = new(0, 0, 11.254f);

        public MatchBall(MatchRoom room) : base(room)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override void Update(double delta)
        {
            var roomMatchInfo = _matchRoom.MatchInformation;

            if (roomMatchInfo.Actor.BallOwner != -1)
                return;

            if (roomMatchInfo.BallVelocity.Length() <= Epsilon)
                return;

            // Velocity 
            roomMatchInfo.BallVelocity = new Vector3(
                roomMatchInfo.BallVelocity.X * Reduction,
                roomMatchInfo.BallVelocity.Y * Reduction,
                (roomMatchInfo.BallVelocity.Z + Gravity * (float)delta) * Reduction
            );

            // Position 
            roomMatchInfo.BallPosition += roomMatchInfo.BallVelocity * (float)delta;

            // Ground 
            if (roomMatchInfo.BallPosition.Z <= BallBoundary.Z)
            {
                roomMatchInfo.BallPosition.Z = BallBoundary.Z;

                roomMatchInfo.BallVelocity = new Vector3(
                    roomMatchInfo.BallVelocity.X,
                    roomMatchInfo.BallVelocity.Y,
                    -roomMatchInfo.BallVelocity.Z * Reduction
                );
            }

            _matchRoom.Players.SendMessage(new BallUpdateMessage(
                roomMatchInfo.BallPosition,
                roomMatchInfo.BallVelocity
            ));
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
                _log.Debug("{id} disposed.", Id);

            base.Dispose(disposing);
        }
    }
}
