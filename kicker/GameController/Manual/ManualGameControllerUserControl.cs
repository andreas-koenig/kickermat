namespace GameController.Manual
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Calibration;
    using Coach;
    using GlobalDataTypes;
    using ObjectDetection;

    /// <summary>
    /// This class implements the user control of the ManualGameController.
    /// </summary>
    public partial class ManualGameControllerUserControl : UserControl
    {
        /// <summary>
        /// Mapping of the players to their corresponding label controls.
        /// Index 0 = Label for maximum position.
        /// Index 1 = Label for minimum position.
        /// Index 2 = Label for current position.
        /// Index 3 = Label for set position.
        /// </summary>
        private readonly Dictionary<Player, Label[]> playerToLabels;

        /// <summary>
        /// Mapping of the players to background colors for their label controls.
        /// Index 0 = Label for maximum position.
        /// Index 1 = Label for minimum position.
        /// Index 2 = Label for current position.
        /// Index 3 = Label for set position.
        /// </summary>
        private readonly Dictionary<Player, Color[]> playerToColors;

        /// <summary>
        /// Mapping of the players to tooltips for their label controls.
        /// Index 0 = Label for maximum position.
        /// Index 1 = Label for minimum position.
        /// Index 2 = Label for current position.
        /// Index 3 = Label for set position.
        /// </summary>
        private readonly Dictionary<Player, string[]> playerToToolTip;

        /// <summary>
        /// Mapping of the bars to their corresponding trackbar controls.
        /// </summary>
        private readonly Dictionary<Player, TrackBar> playerToTrackBar;

        /// <summary>
        /// Callback used with Invoke() to get the set position of a player.
        /// </summary>
        private readonly GetPlayerInvoker getPlayerInvoker;

        /// <summary>
        /// Callback used with Invoke() to get the set angle of a bar.
        /// </summary>
        private readonly GetBarInvoker getBarInvoker;

        /// <summary>
        /// The default background color of labels.
        /// </summary>
        private readonly Color defaultBackColor;

        /// <summary>
        /// The current coach.
        /// </summary>
        private ICoach coach;

        /// <summary>
        /// The current own bar detection.
        /// </summary>
        private IOwnBarDetection detection;

        /// <summary>
        /// The current ball position.
        /// </summary>
        private Position ballpos;

        /// <summary>
        /// The current correction parameters from calibration.
        /// </summary>
        private ICorrectionParams correction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualGameControllerUserControl"/> class.
        /// </summary>
        public ManualGameControllerUserControl()
        {
            this.InitializeComponent();
            this.getPlayerInvoker = new GetPlayerInvoker(this.GetPlayerYPos);
            this.getBarInvoker = new GetBarInvoker(this.GetBarAngle);

            this.defaultBackColor = Color.Transparent;

            this.playerToLabels = new Dictionary<Player, Label[]>();
            this.playerToLabels.Add(Player.Keeper, new Label[] { this.labelKeeperMax, this.labelKeeperMin, this.labelKeeperCur, this.labelKeeperSet });
            this.playerToLabels.Add(Player.DefenseOne, new Label[] { this.labelDefense1Max, this.labelDefense1Min, this.labelDefense1Cur, this.labelDefense1Set });
            this.playerToLabels.Add(Player.DefenseTwo, new Label[] { this.labelDefense2Max, this.labelDefense2Min, this.labelDefense2Cur, this.labelDefense2Set });
            this.playerToLabels.Add(Player.MidfieldOne, new Label[] { this.labelMidfield1Max, this.labelMidfield1Min, this.labelMidfield1Cur, this.labelMidfield1Set });
            this.playerToLabels.Add(Player.MidfieldTwo, new Label[] { this.labelMidfield2Max, this.labelMidfield2Min, this.labelMidfield2Cur, this.labelMidfield2Set });
            this.playerToLabels.Add(Player.MidfieldThree, new Label[] { this.labelMidfield3Max, this.labelMidfield3Min, this.labelMidfield3Cur, this.labelMidfield3Set });
            this.playerToLabels.Add(Player.MidfieldFour, new Label[] { this.labelMidfield4Max, this.labelMidfield4Min, this.labelMidfield4Cur, this.labelMidfield4Set });
            this.playerToLabels.Add(Player.MidfieldFive, new Label[] { this.labelMidfield5Max, this.labelMidfield5Min, this.labelMidfield5Cur, this.labelMidfield5Set });
            this.playerToLabels.Add(Player.StrikerOne, new Label[] { this.labelStriker1Max, this.labelStriker1Min, this.labelStriker1Cur, this.labelStriker1Set });
            this.playerToLabels.Add(Player.StrikerTwo, new Label[] { this.labelStriker2Max, this.labelStriker2Min, this.labelStriker2Cur, this.labelStriker2Set });
            this.playerToLabels.Add(Player.StrikerThree, new Label[] { this.labelStriker3Max, this.labelStriker3Min, this.labelStriker3Cur, this.labelStriker3Set });

            this.playerToTrackBar = new Dictionary<Player, TrackBar>();
            this.playerToTrackBar.Add(Player.Keeper, this.trackBarKeeper);
            this.playerToTrackBar.Add(Player.DefenseOne, this.trackBarDefense1);
            this.playerToTrackBar.Add(Player.DefenseTwo, this.trackBarDefense2);
            this.playerToTrackBar.Add(Player.MidfieldOne, this.trackBarMidfield1);
            this.playerToTrackBar.Add(Player.MidfieldTwo, this.trackBarMidfield2);
            this.playerToTrackBar.Add(Player.MidfieldThree, this.trackBarMidfield3);
            this.playerToTrackBar.Add(Player.MidfieldFour, this.trackBarMidfield4);
            this.playerToTrackBar.Add(Player.MidfieldFive, this.trackBarMidfield5);
            this.playerToTrackBar.Add(Player.StrikerOne, this.trackBarStriker1);
            this.playerToTrackBar.Add(Player.StrikerTwo, this.trackBarStriker2);
            this.playerToTrackBar.Add(Player.StrikerThree, this.trackBarStriker3);

            this.playerToColors = new Dictionary<Player, Color[]>();
            this.playerToToolTip = new Dictionary<Player, string[]>();
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                this.playerToColors.Add(player, new Color[4]);
                this.playerToToolTip.Add(player, new string[4]);
                for (int i = 0; i < 4; i++)
                {
                    this.playerToColors[player][0] = this.defaultBackColor;
                    this.playerToToolTip[player][0] = string.Empty;
                }
            }
        }

        /// <summary>
        /// Delegate to get the set y position of a player.
        /// </summary>
        /// <param name="play">The wanted player.</param>
        /// <returns>The y position of the player.</returns>
        private delegate int GetPlayerInvoker(Player play);

        /// <summary>
        /// Delegate to get the set angle of a bar.
        /// </summary>
        /// <param name="bar">The wanted bar.</param>
        /// <returns>The angle of the bar.</returns>
        private delegate int GetBarInvoker(Bar bar);

        /// <summary>
        /// Updates the seldom changing values from coach and calibration.
        /// </summary>
        /// <param name="coach">The current instance of Coach.</param>
        /// <param name="correction">The current correction parameters from calibration.</param>
        internal void UpdateStaticValues(ICoach coach, ICorrectionParams correction)
        {
            this.coach = coach;
            this.correction = correction;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.UpdateStaticValues2));
            }
            else
            {
                this.UpdateStaticValues2();
            }
        }

        /// <summary>
        /// Updates the ever changing values from the object detection.
        /// </summary>
        /// <param name="detection">The current own bar detection.</param>
        /// <param name="ballpos">The current ball position.</param>
        internal void UpdateDynamicValues(IOwnBarDetection detection, Position ballpos)
        {
            this.detection = detection;
            this.ballpos = ballpos;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.UpdateDynamicValues2));
            }
            else
            {
                this.UpdateDynamicValues2();
            }
        }

        /// <summary>
        /// Returns the set y position for the specified player.
        /// </summary>
        /// <param name="player">The wanted player.</param>
        /// <returns>The currently set y position.</returns>
        internal int GetPlayerYPos(Player player)
        {
            if (this.InvokeRequired)
            {
                return (int)this.Invoke(this.getPlayerInvoker, player);
            }

            return this.playerToTrackBar[player].Value;
        }

        /// <summary>
        /// Returns the set angel for the specified bar.
        /// </summary>
        /// <param name="bar">The wanted bar.</param>
        /// <returns>The currently set angle.</returns>
        internal int GetBarAngle(Bar bar)
        {
            if (this.InvokeRequired)
            {
                return (int)this.Invoke(this.getBarInvoker, bar);
            }

            switch (bar)
            {
                case Bar.Keeper: return this.trackBarKeeperAngle.Value;
                case Bar.Defense: return this.trackBarDefenseAngle.Value;
                case Bar.Midfield: return this.trackBarMidfieldAngle.Value;
                case Bar.Striker: return this.trackBarStrikerAngle.Value;
            }

            return 0;
        }

        /// <summary>
        /// Called via Invoke() to update values from coach and calibration.
        /// </summary>
        private void UpdateStaticValues2()
        {
            if (this.correction != null)
            {
                // Center
                this.labelCenterX.Text = this.correction.PlayingFieldCenter.XPosition.ToString();
                this.labelCenterY.Text = this.correction.PlayingFieldCenter.YPosition.ToString();
            }

            if (this.coach != null)
            {
                // Bar positions
                this.labelKeeperBarPos.Text = this.coach.GetBarXPosition(Bar.Keeper).ToString();
                this.labelDefenseBarPos.Text = this.coach.GetBarXPosition(Bar.Defense).ToString();
                this.labelMidfieldBarPos.Text = this.coach.GetBarXPosition(Bar.Midfield).ToString();
                this.labelStrikerBarPos.Text = this.coach.GetBarXPosition(Bar.Striker).ToString();

                foreach (KeyValuePair<Player, Label[]> kv in this.playerToLabels)
                {
                    // Player max positions
                    kv.Value[0].Text = this.coach.GetPlayerMaxYPosition(kv.Key).ToString();

                    // Player set and min positions
                    kv.Value[3].Text = kv.Value[1].Text = this.coach.GetPlayerMinYPosition(kv.Key).ToString();
                }

                foreach (KeyValuePair<Player, TrackBar> kv in this.playerToTrackBar)
                {
                    // Trackbar max values
                    kv.Value.Maximum = this.coach.GetPlayerMaxYPosition(kv.Key);

                    // Trackbar current and min values
                    kv.Value.Value = kv.Value.Minimum = this.coach.GetPlayerMinYPosition(kv.Key);
                }

                this.ColorStaticInvalidBarPositions();
                this.ColorStaticInvalidMinMaxPositions();
            }
        }

        /// <summary>
        /// Sets the background color for invalid bar positions.
        /// </summary>
        private void ColorStaticInvalidBarPositions()
        {
            if (this.coach != null)
            {
                // TODO: Checks with bar position  
            } 
        }

        /// <summary>
        /// Sets the background color for invalid player min max positions.
        /// </summary>
        private void ColorStaticInvalidMinMaxPositions()
        {
            if (this.coach != null)
            {
                // Store color and tooltip for invalid positions
                foreach (Player player in Enum.GetValues(typeof(Player)))
                {
                    int minpos = this.coach.GetPlayerMinYPosition(player);
                    int maxpos = this.coach.GetPlayerMaxYPosition(player);

                    if (minpos > maxpos)
                    {
                        this.playerToColors[player][0] = this.playerToColors[player][1] = KickerColors.OpponentBarColor;
                        this.playerToToolTip[player][0] = "The maximum Y position of " + player.ToString() + " is smaller than its minimum Y position";
                        this.playerToToolTip[player][1] = "The minimum Y position of " + player.ToString() + " is greater than its maximum Y position";
                    }
                }

                // TODO: More checks        
            }

            // Set stored values to the labels
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                this.playerToLabels[player][0].BackColor = this.playerToColors[player][0];
                this.playerToLabels[player][1].BackColor = this.playerToColors[player][1];
                this.labelsToolTip.SetToolTip(this.playerToLabels[player][0], this.playerToToolTip[player][0]);
                this.labelsToolTip.SetToolTip(this.playerToLabels[player][1], this.playerToToolTip[player][1]);
            }
        }

        /// <summary>
        /// Called via Invoke() to update the values from the object detection.
        /// </summary>
        private void UpdateDynamicValues2()
        {
            if (this.ballpos != null)
            {
                this.labelBallX.Text = this.ballpos.XPosition.ToString();
                this.labelBallY.Text = this.ballpos.YPosition.ToString();
                this.labelBallValid.Text = this.ballpos.Valid.ToString();

                this.ColorDynamicBallBetweenBars();
                this.ColorDynamicPlayerWithBall();
            }

            if (this.detection != null)
            {
                foreach (KeyValuePair<Player, Label[]> kv in this.playerToLabels)
                {
                    Position pos = this.detection.GetPlayerPosition(kv.Key);
                    if (pos != null)
                    {
                        kv.Value[2].Text = pos.YPosition.ToString();
                    }
                }

                this.ColorDynamicInvalidPlayerPositions();
            }
        }

        /// <summary>
        /// Sets the background color of the labels that show the ball position.
        /// </summary>
        private void ColorDynamicBallBetweenBars()
        {
            Color color = KickerColors.InvalidBallColor;
            if (this.ballpos == null)
            {
                color = this.defaultBackColor;
            }
            else if (this.ballpos.Valid) 
            {
                color = KickerColors.BallColor;
            }

            this.labelBallX.BackColor = color;
            this.labelBallY.BackColor = color;
            this.labelBallValid.BackColor = color;

            this.labelBallBars1.BackColor = this.defaultBackColor;
            this.labelBallBars2.BackColor = this.defaultBackColor;
            this.labelBallBars3.BackColor = this.defaultBackColor;
            this.labelBallBars4.BackColor = this.defaultBackColor;
            this.labelBallBars5.BackColor = this.defaultBackColor;
            this.labelsToolTip.SetToolTip(this.labelBallBars1, string.Empty);
            this.labelsToolTip.SetToolTip(this.labelBallBars2, string.Empty);
            this.labelsToolTip.SetToolTip(this.labelBallBars3, string.Empty);
            this.labelsToolTip.SetToolTip(this.labelBallBars4, string.Empty);
            this.labelsToolTip.SetToolTip(this.labelBallBars5, string.Empty);

            if (this.coach != null)
            {
                if (this.ballpos.XPosition < this.coach.GetBarXPosition(Bar.Striker))
                {
                    this.labelBallBars1.BackColor = color;
                    this.labelsToolTip.SetToolTip(
                        this.labelBallBars1,
                        "Ball X Position is between left playing field border and " + Bar.Striker.ToString());
                }
                else if (this.ballpos.XPosition < this.coach.GetBarXPosition(Bar.Midfield))
                {
                    this.labelBallBars2.BackColor = color;
                    this.labelsToolTip.SetToolTip(
                        this.labelBallBars3,
                        "Ball X Position is between " + Bar.Striker.ToString() + " and " + Bar.Midfield.ToString());
                }
                else if (this.ballpos.XPosition < this.coach.GetBarXPosition(Bar.Defense))
                {
                    this.labelBallBars3.BackColor = color;
                    this.labelsToolTip.SetToolTip(
                        this.labelBallBars3,
                        "Ball X Position is between " + Bar.Midfield.ToString() + " and " + Bar.Defense.ToString());
                }
                else if (this.ballpos.XPosition < this.coach.GetBarXPosition(Bar.Keeper))
                {
                    this.labelBallBars4.BackColor = color;
                    this.labelsToolTip.SetToolTip(
                        this.labelBallBars4,
                        "Ball X Position is between " + Bar.Defense.ToString() + " and " + Bar.Keeper.ToString());
                }
                else
                {
                    this.labelBallBars5.BackColor = color;
                    this.labelsToolTip.SetToolTip(
                        this.labelBallBars5,
                        "Ball X Position is between " + Bar.Keeper.ToString() + " and right playing field border");
                }                
            }
        }

        /// <summary>
        /// Sets the background color for the players around the Y position of the ball.
        /// </summary>
        private void ColorDynamicPlayerWithBall()
        {
            Color color = KickerColors.InvalidBallColor;
            if (this.ballpos == null)
            {
                color = this.defaultBackColor;               
            }
            else if (this.ballpos.Valid)
            {
                color = KickerColors.BallColor;
            }

            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                if (this.coach != null && this.coach.IsYPositionValid(player, this.ballpos.YPosition))
                {
                    // Set highlighted values for affected players
                    string tt = "Ball Y Position is between " + player.ToString() + " minimum and max position";
                    this.playerToLabels[player][0].BackColor = color;
                    this.playerToLabels[player][1].BackColor = color;                    
                    this.labelsToolTip.SetToolTip(this.playerToLabels[player][0], tt);
                    this.labelsToolTip.SetToolTip(this.playerToLabels[player][1], tt);
                }
                else
                {
                    // Set stored values for other players
                    this.playerToLabels[player][0].BackColor = this.playerToColors[player][0];
                    this.playerToLabels[player][1].BackColor = this.playerToColors[player][1];
                    this.labelsToolTip.SetToolTip(this.playerToLabels[player][0], string.Empty);
                    this.labelsToolTip.SetToolTip(this.playerToLabels[player][1], string.Empty);
                }
            }
        }

        /// <summary>
        /// Sets the background color for invalid player positions.
        /// </summary>
        private void ColorDynamicInvalidPlayerPositions()
        {
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                if (!this.playerToLabels[player][2].Text.Trim().Equals(string.Empty))
                {
                    int pos = int.Parse(this.playerToLabels[player][2].Text);
                    if (this.coach != null && !this.coach.IsYPositionValid(player, pos))
                    {                        
                        // Set highlighted values for current position
                        this.playerToLabels[player][2].BackColor = KickerColors.OpponentBarColor;
                        this.labelsToolTip.SetToolTip(
                            this.playerToLabels[player][2],
                            "Current Y Position of " + player.ToString() + " is not valid");
                        continue;
                    }
                }

                // Set values for current position from stored values
                this.playerToLabels[player][2].BackColor = this.playerToColors[player][2];
                this.labelsToolTip.SetToolTip(this.playerToLabels[player][2], this.playerToToolTip[player][2]);                
            }
        }
        
        /// <summary>
        /// Sets the background color for the active player of a bar.
        /// </summary>
        /// <param name="bar">The active bar.</param>
        /// <param name="player">The active player.</param>
        private void ColorActivePlayer(Bar bar, Player player)
        {
            foreach (Player p in bar.GetPlayers())
            {
                if (!p.Equals(player))
                {                   
                    // Store default values for inactive players
                    this.playerToColors[p][2] = this.playerToColors[p][3] = this.defaultBackColor;
                    this.playerToToolTip[p][2] = this.playerToToolTip[p][3] = string.Empty;

                    // Set only "set position" for inactive players
                    this.playerToLabels[p][3].BackColor = this.playerToColors[p][3];
                    this.labelsToolTip.SetToolTip(this.playerToLabels[p][3], this.playerToToolTip[p][3]);
                }
            }

            // Store highlighted values for active player            
            this.playerToColors[player][2] = this.playerToColors[player][3] = KickerColors.OwnBarColor;
            this.playerToToolTip[player][2] = this.playerToToolTip[player][3] = player.ToString() + " is the active player";

            // Set only "set position" for active player
            this.playerToLabels[player][3].BackColor = this.playerToColors[player][3];
            this.labelsToolTip.SetToolTip(this.playerToLabels[player][3], this.playerToToolTip[player][3]);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarKeeper control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarKeeper_Scroll(object sender, System.EventArgs e)
        {
            this.labelKeeperSet.Text = this.trackBarKeeper.Value.ToString();
            this.ColorActivePlayer(Bar.Keeper, Player.Keeper);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarDefense1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarDefense1_Scroll(object sender, System.EventArgs e)
        {
            this.labelDefense1Set.Text = this.trackBarDefense1.Value.ToString();
            this.ColorActivePlayer(Bar.Defense, Player.DefenseOne);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarDefense2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarDefense2_Scroll(object sender, System.EventArgs e)
        {
            this.labelDefense2Set.Text = this.trackBarDefense2.Value.ToString();
            this.ColorActivePlayer(Bar.Defense, Player.DefenseTwo);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarMidfield1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarMidfield1_Scroll(object sender, System.EventArgs e)
        {
            this.labelMidfield1Set.Text = this.trackBarMidfield1.Value.ToString();
            this.ColorActivePlayer(Bar.Midfield, Player.MidfieldOne);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarMidfield2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarMidfield2_Scroll(object sender, System.EventArgs e)
        {
            this.labelMidfield2Set.Text = this.trackBarMidfield2.Value.ToString();
            this.ColorActivePlayer(Bar.Midfield, Player.MidfieldTwo);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarMidfield3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarMidfield3_Scroll(object sender, System.EventArgs e)
        {
            this.labelMidfield3Set.Text = this.trackBarMidfield3.Value.ToString();
            this.ColorActivePlayer(Bar.Midfield, Player.MidfieldThree);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarMidfield4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarMidfield4_Scroll(object sender, System.EventArgs e)
        {
            this.labelMidfield4Set.Text = this.trackBarMidfield4.Value.ToString();
            this.ColorActivePlayer(Bar.Midfield, Player.MidfieldFour);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarMidfield5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarMidfield5_Scroll(object sender, System.EventArgs e)
        {
            this.labelMidfield5Set.Text = this.trackBarMidfield5.Value.ToString();
            this.ColorActivePlayer(Bar.Midfield, Player.MidfieldFive);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarStriker1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarStriker1_Scroll(object sender, System.EventArgs e)
        {
            this.labelStriker1Set.Text = this.trackBarStriker1.Value.ToString();
            this.ColorActivePlayer(Bar.Striker, Player.StrikerOne);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarStriker2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarStriker2_Scroll(object sender, System.EventArgs e)
        {
            this.labelStriker2Set.Text = this.trackBarStriker2.Value.ToString();
            this.ColorActivePlayer(Bar.Striker, Player.StrikerTwo);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarStriker3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarStriker3_Scroll(object sender, System.EventArgs e)
        {
            this.labelStriker3Set.Text = this.trackBarStriker3.Value.ToString();
            this.ColorActivePlayer(Bar.Striker, Player.StrikerThree);
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarKeeperAngle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarKeeperAngle_Scroll(object sender, System.EventArgs e)
        {
            this.labelKeeperBarAngle.Text = this.trackBarKeeperAngle.Value.ToString();
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarDefenseAngle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarDefenseAngle_Scroll(object sender, System.EventArgs e)
        {
            this.labelDefenseBarAngle.Text = this.trackBarDefenseAngle.Value.ToString();
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarMidfieldAngle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarMidfieldAngle_Scroll(object sender, System.EventArgs e)
        {
            this.labelMidfieldBarAngle.Text = this.trackBarMidfieldAngle.Value.ToString();
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarStrikerAngle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarStrikerAngle_Scroll(object sender, System.EventArgs e)
        {
            this.labelStrikerBarAngle.Text = this.trackBarStrikerAngle.Value.ToString();
        }        
    }
}
