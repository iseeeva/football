using Sobee.TestServer.Match;
using Sobee.TestServer.Messages.Ball;
using System.Numerics;

namespace Sobee.TestServer.MatchComponents
{
    public class MatchBallComponent : MatchComponent<MatchRoom>
    {
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

        public MatchBallComponent()
        {

        }

        #region Lifecycle
        protected override void OnUpdate(double delta)
        {
            if (Owner == null)
            {
                _log.Warning("Owner is null. Component aborted.");
                Stop();

                return;
            }

            var matchInfo = Owner.MatchInformation;
            var ballOwnerInfo = matchInfo.GetPlayer(matchInfo.BallOwner);
            var dt = (float)delta;

            if (ballOwnerInfo != null)
            {
                //matchInfo.BallPosition += ballOwnerInfo.Velocity * dt;
                //matchInfo.BallVelocity = Vector3.Zero;
            }
            else
            {
                if (matchInfo.BallVelocity.Length() <= Epsilon)
                    return;

                matchInfo.BallVelocity = new Vector3(
                    matchInfo.BallVelocity.X * Reduction,
                    matchInfo.BallVelocity.Y * Reduction,
                    (matchInfo.BallVelocity.Z + Gravity * dt) * Reduction
                );

                matchInfo.BallPosition += matchInfo.BallVelocity * dt;

                if (matchInfo.BallPosition.Z <= BallBoundary.Z)
                {
                    matchInfo.BallPosition.Z = BallBoundary.Z;
                    matchInfo.BallVelocity = new Vector3(
                        matchInfo.BallVelocity.X,
                        matchInfo.BallVelocity.Y,
                        -matchInfo.BallVelocity.Z * Reduction
                    );
                }

                Owner.Players.SendMessage(new BallUpdateTxMessage(
                    matchInfo.BallPosition,
                    matchInfo.BallVelocity
                ));
            }
            #endregion
        }
    }
}