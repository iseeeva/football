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
        public float Epsilon { get; init; } = 10f;

        /// <summary>Gravity affecting the ball's vertical movement</summary>
        public float Gravity { get; init; } = -9.81f * 100f * 2.5f;

        /// <summary>Reduction factor for ball speed over time</summary>
        public float Reduction { get; init; } = 0.985f;

        /// <summary>Radius for ball collision detection</summary>
        public double BallCollisionRadius { get; init; } = 15;

        /// <summary>Maximum speed of the ball</summary>
        public double BallMaxSpeed { get; init; } = 2000;

        /// <summary>Boundary for the ball (Z axis is ground level)</summary>
        public Vector3 BallBoundary { get; init; } = new(0, 0, 11.254f);

        public MatchBall(MatchRoom room) : base(room)
        {
            _log.Debug("{id} initialized.", Id);
        }

        public override Task Update(double delta)
        {
            var matchInfo = _matchRoom.MatchInformation;

            if (matchInfo.BallVelocity.Length() > Epsilon)
            {
                matchInfo.BallVelocity = new Vector3(
                    matchInfo.BallVelocity.X * Reduction,
                    matchInfo.BallVelocity.Y * Reduction,
                    (matchInfo.BallVelocity.Z + Gravity * (float)delta) * Reduction
                );

                matchInfo.BallPosition += matchInfo.BallVelocity * (float)delta;

                if (matchInfo.BallPosition.Z <= BallBoundary.Z)
                {
                    matchInfo.BallPosition.Z = BallBoundary.Z;

                    matchInfo.BallVelocity = new Vector3(
                        matchInfo.BallVelocity.X,
                        matchInfo.BallVelocity.Y,
                        -matchInfo.BallVelocity.Z * Reduction
                    );
                }

                _matchRoom.Players.SendMessage(new BallUpdate(
                    matchInfo.BallPosition,
                    matchInfo.BallVelocity
                ));
            }

            return Task.CompletedTask;
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
