using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GameController
{
    using System.Windows.Forms;
    using GlobalDataTypes;
    using GlobalDataTypes.Buffers;

    /// <summary>
    /// This class implements the DefaultGameController used for the playing logic.
    /// </summary>
    public sealed class DualGameController : BasicGameController
    {
        /// <summary>
        /// The bar for a action.
        /// </summary>
        private Bar actionBar;

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        protected override void Play(Position ballpos)
        {
            if (ballpos.Valid == true)
            {
                this.DetermineActionBar(ballpos);
                this.SetKeeperPosition(ballpos);
                this.SetDefensePosition(ballpos);
                this.SetMidfieldPosition(ballpos);
                this.SetStrikerPosition(ballpos);
                this.ShootBallForward(ballpos);
            }
        }

        /// <summary>
        /// Determines the action bar.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void DetermineActionBar(Position ballpos)
        {
            if (Coach.GetBarXPosition(Bar.Keeper) > ballpos.XPosition &&
                ballpos.XPosition > Coach.GetBarXPosition(Bar.Defense))
            {
                this.actionBar = Bar.Keeper;
            }
            else if (Coach.GetBarXPosition(Bar.Defense) > ballpos.XPosition &&
                     ballpos.XPosition > Coach.GetBarXPosition(Bar.Midfield))
            {
                this.actionBar = Bar.Defense;
            }
            else if (Coach.GetBarXPosition(Bar.Midfield) > ballpos.XPosition &&
                     ballpos.XPosition > Coach.GetBarXPosition(Bar.Striker))
            {
                this.actionBar = Bar.Midfield;
            }
            else if (Coach.GetBarXPosition(Bar.Striker) > ballpos.XPosition)
            {
                this.actionBar = Bar.Striker;
            }
        }

        /// <summary>
        /// Shoots the ball forward.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void ShootBallForward(Position ballpos)
        {
            if (!Coach.IsYPositionValid(this.actionBar, ballpos.YPosition))
            {
                return;
            }

            switch (this.actionBar)
            {
                case Bar.Keeper:
                    if (ballpos.XPosition >= Coach.GetBarXPosition(this.actionBar) - this.ShootingRange &&
                        ballpos.XPosition < Coach.GetBarXPosition(this.actionBar))
                    {
                        Coach.SetPlayerAngle(Bar.Keeper, 90);
                    }

                    break;
                case Bar.Defense:
                    if (ballpos.XPosition >= Coach.GetBarXPosition(this.actionBar) - this.ShootingRange &&
                        ballpos.XPosition < Coach.GetBarXPosition(this.actionBar))
                    {
                        Coach.SetPlayerAngle(Bar.Defense, 90);
                    }

                    break;
                case Bar.Midfield:
                    if (ballpos.XPosition >= Coach.GetBarXPosition(this.actionBar) - this.ShootingRange &&
                        ballpos.XPosition < Coach.GetBarXPosition(this.actionBar))
                    {
                        Coach.SetPlayerAngle(Bar.Midfield, 90);
                    }

                    break;
                case Bar.Striker:
                    if (ballpos.XPosition >= Coach.GetBarXPosition(this.actionBar) - this.ShootingRange &&
                        ballpos.XPosition < Coach.GetBarXPosition(this.actionBar))
                    {
                        Coach.SetPlayerAngle(Bar.Striker, 90);
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the keeper position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetKeeperPosition(Position ballpos)
        {
            if (Coach.GetBarXPosition(Bar.Keeper) > ballpos.XPosition)
            {
                Coach.SetPlayerAngleBlock(Bar.Keeper);
            }

            int newKeeperPosition = ballpos.YPosition;

            if (Coach.GoalTop <= ballpos.YPosition)
            {
                newKeeperPosition = Coach.GetPlayerMinYPosition(Player.Keeper);
            }
            else if (Coach.GoalBottom >= ballpos.YPosition)
            {
                newKeeperPosition = Coach.GetPlayerMaxYPosition(Player.Keeper);
            }

            Coach.MovePlayerToYPosition(Player.Keeper, newKeeperPosition);
        }

        /// <summary>
        /// Sets the defense position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetDefensePosition(Position ballpos)
        {
            if (Coach.GetBarXPosition(Bar.Defense) > ballpos.XPosition)
            {
                Coach.SetPlayerAngleBlock(Bar.Defense);
            }
            else
            {
                Coach.SetPlayerAnglePass(Bar.Defense);
            }

            Player playerToMove = Player.DefenseOne;

            if (Coach.IsYPositionValid(Player.DefenseOne, ballpos.YPosition))
            {
                playerToMove = Player.DefenseOne;
            }
            else if (Coach.IsYPositionValid(Player.DefenseTwo, ballpos.YPosition))
            {
                playerToMove = Player.DefenseTwo;
            }

            Coach.MovePlayerToYPosition(playerToMove, ballpos.YPosition);
        }

        /// <summary>
        /// Sets the midfield position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetMidfieldPosition(Position ballpos)
        {
            if (Coach.GetBarXPosition(Bar.Midfield) > ballpos.XPosition)
            {
                Coach.SetPlayerAngleBlock(Bar.Midfield);
            }
            else
            {
                Coach.SetPlayerAnglePass(Bar.Midfield);
            }

            Player playerToMove = Player.MidfieldOne;

            if (Coach.IsYPositionValid(Player.MidfieldOne, ballpos.YPosition))
            {
                playerToMove = Player.MidfieldOne;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldTwo, ballpos.YPosition))
            {
                playerToMove = Player.MidfieldTwo;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldThree, ballpos.YPosition))
            {
                playerToMove = Player.MidfieldThree;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldFour, ballpos.YPosition))
            {
                playerToMove = Player.MidfieldFour;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldFive, ballpos.YPosition))
            {
                playerToMove = Player.MidfieldFive;
            }

            Coach.MovePlayerToYPosition(playerToMove, ballpos.YPosition);
        }

        /// <summary>
        /// Sets the striker position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetStrikerPosition(Position ballpos)
        {
            if (Coach.GetBarXPosition(Bar.Striker) > ballpos.XPosition)
            {
                Coach.SetPlayerAngleBlock(Bar.Striker);
            }
            else
            {
                Coach.SetPlayerAnglePass(Bar.Striker);
            }

            Player playerToMove = Player.StrikerOne;

            if (Coach.IsYPositionValid(Player.StrikerOne, ballpos.YPosition))
            {
                playerToMove = Player.StrikerOne;
            }
            else if (Coach.IsYPositionValid(Player.StrikerTwo, ballpos.YPosition))
            {
                playerToMove = Player.StrikerTwo;
            }
            else if (Coach.IsYPositionValid(Player.StrikerThree, ballpos.YPosition))
            {
                playerToMove = Player.StrikerThree;
            }

            Coach.MovePlayerToYPosition(playerToMove, ballpos.YPosition);
        }
    }
}
