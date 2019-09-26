namespace Calibration.Fixed
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using GameProperties;
    using Calibration.Base;
    using static GameProperties.Bar;


    /// <summary>
    /// Alternative class for calibrating the image processing from fixed values.
    /// </summary>
    public sealed class FixedCalibration : BasicCalibration<FixedCalibrationSettings>
    {
        /// <summary>
        /// The handles to the labels for all players minumim and maximum positions.
        /// </summary>
        private readonly Dictionary<Player, int[]> labelHandles;

        /// <summary>
        /// The minimum Y positions of all players.
        /// </summary>
        private readonly Dictionary<Player, Position> playerMinPos;

        /// <summary>
        /// The maximum Y positions of all players.
        /// </summary>
        private readonly Dictionary<Player, Position> playerMaxPos;

        /// <summary>
        /// The calculated X positions of all bars.
        /// </summary>
        private readonly Dictionary<Bar, int> barPos;

        /// <summary>
        /// Initializes a new instance of the FixedCalibration class.
        /// </summary>
        public FixedCalibration()
            : base()
        {
            this.playerMinPos = new Dictionary<Player, Position>();
            this.playerMaxPos = new Dictionary<Player, Position>();
            this.labelHandles = new Dictionary<Player, int[]>();
            this.barPos = new Dictionary<Bar, int>();

            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                this.playerMinPos[player] = new Position();
                this.playerMaxPos[player] = new Position();
                this.labelHandles[player] = new int[2];
                //this.labelHandles[player][0] = FormImageDisplay.Instance.Labels.Create(Color.White, player.ShortName() + "-Min", false);
                //this.labelHandles[player][1] = FormImageDisplay.Instance.Labels.Create(Color.White, player.ShortName() + "-Max", false);                
            }
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public override void Dispose()
        {
            //foreach (Player player in Enum.GetValues(typeof(Player)))
            //{
            //    FormImageDisplay.Instance.Labels.Delete(this.labelHandles[player][0]);
            //    FormImageDisplay.Instance.Labels.Delete(this.labelHandles[player][1]);
            //}

            base.Dispose();
        }
        
        /// <summary>
        /// Called by the handler method for the DoWork event of the <c>calibrationThread</c>
        /// to do the actual calibration.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        /// <param name="minPositions">Out parameter for the minimum positions of the players.</param>
        /// <param name="maxPositions">Out parameter for the maximum positions of the players.</param>
        /// <returns>if the calibration has been successful.</returns>
        protected override bool DoCalibration(object sender, DoWorkEventArgs e, Dictionary<Player, Position> minPositions, Dictionary<Player, Position> maxPositions)
        {
            // Calculate the position from settings
            this.ReCalcPlayerPositions();

            // Use calculated position
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                minPositions[player] = this.playerMinPos[player];
                maxPositions[player] = this.playerMaxPos[player];
            }

            return true;
        }

        /// <summary>
        /// Called by the handler method for the DoWork event of the <c>calibrationThread</c>
        /// after evaluating the calibration values. Must be overridden by derived classes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        protected override void AfterCalibration(object sender, DoWorkEventArgs e)
        {
            // Nothing to do
        }

        /// <summary>
        /// Calculates the player positions from the calibrations settings.
        /// </summary>
        private void ReCalcPlayerPositions()
        {
            //this.barPos[Bar.Keeper] = this.Settings.PlayingFieldWidth - this.Settings.KeeperBarPosition + this.Settings.PlayingFieldXOffset;
            //this.barPos[Bar.Defense] = this.Settings.PlayingFieldWidth - this.Settings.DefenseBarPosition + this.Settings.PlayingFieldXOffset;
            //this.barPos[Bar.Midfield] = this.Settings.PlayingFieldWidth - this.Settings.MidfieldBarPosition + this.Settings.PlayingFieldXOffset;
            //this.barPos[Bar.Striker] = this.Settings.OpponentStrikerBarPosition + this.Settings.PlayingFieldXOffset;

            // Set the bar position as x position for each player on that bar
            foreach (Bar bar in Enum.GetValues(typeof(Bar)))
            {
                if (bar.barSelection != BarType.All)
                {
                    foreach (Player player in bar.GetPlayers())
                    {
                        //this.playerMinPos[player].XPosition = this.barPos[bar];
                        //this.playerMaxPos[player].XPosition = this.barPos[bar];
                    }
                }
            }
            
            // Set the min y positions from the settings
            //this.playerMinPos[Player.Keeper].YPosition = this.Settings.KeeperMin;
            //this.playerMinPos[Player.DefenseOne].YPosition = this.Settings.DefenseOneMin;
            //this.playerMinPos[Player.DefenseTwo].YPosition = this.Settings.DefenseTowMin;
            //this.playerMinPos[Player.MidfieldOne].YPosition = this.Settings.MidfieldOneMin;
            //this.playerMinPos[Player.MidfieldTwo].YPosition = this.Settings.MidfieldTwoMin;
            //this.playerMinPos[Player.MidfieldThree].YPosition = this.Settings.MidfieldThreeMin;
            //this.playerMinPos[Player.MidfieldFour].YPosition = this.Settings.MidfieldFourMin;                        
            //this.playerMinPos[Player.MidfieldFive].YPosition = this.Settings.MidfieldFiveMin;
            //this.playerMinPos[Player.StrikerOne].YPosition = this.Settings.StrikerOneMin;
            //this.playerMinPos[Player.StrikerTwo].YPosition = this.Settings.StrikerTwoMin;
            //this.playerMinPos[Player.StrikerThree].YPosition = this.Settings.StrikerThreeMin;

            // Set the max y positions from the settings
            //this.playerMaxPos[Player.Keeper].YPosition = this.Settings.KeeperMax;
            //this.playerMaxPos[Player.DefenseOne].YPosition = this.Settings.DefenseOneMax;
            //this.playerMaxPos[Player.DefenseTwo].YPosition = this.Settings.DefenseTowMax;
            //this.playerMaxPos[Player.MidfieldOne].YPosition = this.Settings.MidfieldOneMax;
            //this.playerMaxPos[Player.MidfieldTwo].YPosition = this.Settings.MidfieldTwoMax;
            //this.playerMaxPos[Player.MidfieldThree].YPosition = this.Settings.MidfieldThreeMax;
            //this.playerMaxPos[Player.MidfieldFour].YPosition = this.Settings.MidfieldFourMax;
            //this.playerMaxPos[Player.MidfieldFive].YPosition = this.Settings.MidfieldFiveMax;
            //this.playerMaxPos[Player.StrikerOne].YPosition = this.Settings.StrikerOneMax;
            //this.playerMaxPos[Player.StrikerTwo].YPosition = this.Settings.StrikerTwoMax;
            //this.playerMaxPos[Player.StrikerThree].YPosition = this.Settings.StrikerThreeMax;
        }
    }
}
