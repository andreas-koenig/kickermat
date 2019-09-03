namespace Calibration.Base
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using Communication.Calibration;
    using Communication.Manager;
    using System.Drawing;
    using Game;
    using static Game.Bar;
    using Moq;

    /// <summary>
    /// Base class for calibration implementations.
    /// </summary>
    /// <typeparam name="TSettings">The type of the Settings.</typeparam>
    public abstract class BasicCalibration<TSettings> : ICalibration, IDisposable
        where TSettings : BasicCalibrationSettings
    {
        static int HARD_CODED_VALUE = 100;

        /// <summary>
        /// The handles to all calibration labels.
        /// </summary>
        private readonly int[] labelHandles = new int[23];

        /// <summary>
        /// The position values of all calibration labels.
        /// </summary>
        private readonly Position[] labelValues = new Position[23];

        /// <summary>
        /// Background worker for calibration.
        /// </summary>
        private BackgroundWorker calibrationThread;

        /// <summary>
        /// Initializes a new instance of the BasicCalibration class.
        /// </summary>
        protected BasicCalibration()
        {
            this.State = CalibrationState.Running;
            this.calibrationThread = new BackgroundWorker();
            this.calibrationThread.WorkerSupportsCancellation = true;
            this.calibrationThread.DoWork += this.DoWork;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public CalibrationState State { get; protected set; }

        /// <summary>
        /// Gets the reference to the created correction parameters.
        /// </summary>
        public ICorrectionParams Params { get; private set; }

        /// <summary>
        /// Gets the reference to the calibration controller.
        /// </summary>
        protected ICalibrationControl Controller { get; private set; }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public virtual void Cancel()
        {
            this.State = CalibrationState.Error;
            this.calibrationThread.CancelAsync();
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public virtual void Execute()
        {
            if ((this.calibrationThread != null) && this.calibrationThread.IsBusy)
            {
                this.calibrationThread.CancelAsync();
                while (this.calibrationThread.IsBusy)
                {
                    //TODO: Substitue, as this is specific to windows forms
                    //Application.DoEvents();
                }
            }

            this.calibrationThread.RunWorkerAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (this.calibrationThread != null)
            {
                if (this.calibrationThread.IsBusy)
                {
                    this.calibrationThread.CancelAsync();
                    while (this.calibrationThread.IsBusy)
                    {
                        //TODO: Substitue, as this is specific to windows forms
                        //Application.DoEvents();
                    }
                }

                this.calibrationThread.Dispose();
                this.calibrationThread = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            //TODO: Store Settings
            //this.Settings = SettingsSerializer.LoadSettingsFromXml<TSettings>(xmlFileName);
            //this.Settings.Validate();
        }

        /// <summary>
        /// Saves the configuration to a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void SaveConfiguration(string xmlFileName)
        {   //TODO: Store Settings
            //SettingsSerializer.SaveSettingsToXml(this.Settings, xmlFileName);
        }

        /// <summary>
        /// Called by the handler method for the DoWork event of the <c>calibrationThread</c>
        /// to do the actual calibration. Must be overridden by derived classes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        /// <param name="minPositions">Out parameter for the minimum positions of the players.</param>
        /// <param name="maxPositions">Out parameter for the maximum positions of the players.</param>
        /// <returns>if the calibration has been successful.</returns>
        protected abstract bool DoCalibration(object sender, DoWorkEventArgs e, Dictionary<Player, Position> minPositions, Dictionary<Player, Position> maxPositions);

        /// <summary>
        /// Called by the handler method for the DoWork event of the <c>calibrationThread</c>
        /// after evaluating the calibration values. Must be overridden by derived classes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        protected abstract void AfterCalibration(object sender, DoWorkEventArgs e);

        /// <summary>
        /// Calculates the playing field center from calibration settings.
        /// </summary>
        /// <param name="settings">The calibration settings.</param>
        /// <returns>The position of the playing field center.</returns>
        private static Position CalculateCenter(BasicCalibrationSettings settings)
        {
            return new Position(
                (settings.PlayingFieldWidth / 2) + settings.PlayingFieldXOffset,
                (settings.PlayingFieldHeight / 2) + settings.PlayingFieldYOffset,
                true, true);
        }

        /// <summary>
        /// Calculates the bottom right corner from calibration settings.
        /// </summary>
        /// <param name="settings">The calibration settings.</param>
        /// <returns>The position of the bottom right playing field corner.</returns>
        private static Position CalculateBottomRight(BasicCalibrationSettings settings)
        {
            Position br = new Position(settings.PlayingFieldXOffset, settings.PlayingFieldYOffset, true, true);
            br.xPosition += settings.PlayingFieldWidth;
            br.yPosition += settings.PlayingFieldHeight;
            return br;
        }

        /// <summary>
        /// Calculates the parallax and offset correction for the playing field parameters.
        /// </summary>
        /// <param name="settings">The settings fro calibration.</param>
        /// <param name="coach">An instance of <see cref="ICoachInit"/> to assign the parameters.</param>
        /// <param name="corrparams">An instance of <see cref="CorrectionParams"/> to assign the parameters.</param>
        private static void CalculateCorrectionParams(BasicCalibrationSettings settings, CorrectionParams corrparams)
        {
            int cameraLongZ = settings.CameraZDistance;
            int playerShortZ = settings.PlayerZDistance;
            int ballshortZ = settings.BallDiameter;

            // Use playing field center as point of reference
            Position center = CalculateCenter(settings);


            // Calculate corrected playing field dimensions
            //int corrWidth = bottRight.xPosition - corrOffset.xPosition;
            //int corrHeight = bottRight.yPosition - corrOffset.yPosition;

            // Caluculate Y correction of the vertical values
            //int corrGoalTop = settings.GoalTop;
            //int corrGoalBottom = settings.PlayingFieldHeight - settings.GoalTop;
            //int corrGoalAreaTop = settings.GoalAreaTop;
            //int corrGoalAreaBottom = settings.PlayingFieldHeight - settings.GoalAreaTop;
            //int corrPenaltyBoxTop = settings.PenaltyBoxTop;
            //int corrPenaltyBoxBottom = settings.PlayingFieldHeight - settings.PenaltyBoxTop;
            //corrGoalTop = SwissKnife.ParallaxCorrection(cameraLongZ, playerShortZ, center.YPosition, corrGoalTop);
            //corrGoalBottom = SwissKnife.ParallaxCorrection(cameraLongZ, playerShortZ, center.YPosition, corrGoalBottom);
            //corrGoalAreaTop = SwissKnife.ParallaxCorrection(cameraLongZ, playerShortZ, center.YPosition, corrGoalAreaTop);
            //corrGoalAreaBottom = SwissKnife.ParallaxCorrection(cameraLongZ, playerShortZ, center.YPosition, corrGoalAreaBottom);
            //corrPenaltyBoxTop = SwissKnife.ParallaxCorrection(cameraLongZ, playerShortZ, center.YPosition, corrPenaltyBoxTop);
            //corrPenaltyBoxBottom = SwissKnife.ParallaxCorrection(cameraLongZ, playerShortZ, center.YPosition, corrPenaltyBoxBottom);

            // Set values for coach
            //coach.SetFieldCenter(center);
            //coach.SetFieldOffset(corrOffset);


            // Set values for correction
            corrparams.BallDiameter = settings.BallDiameter;
            corrparams.PlayerZDistance = settings.PlayerZDistance;
            corrparams.CameraZDistance = settings.CameraZDistance;
            corrparams.PlayingFieldCenter = center;
            //corrparams.PlayingFieldOffset = corrOffset;
            corrparams.PlayingFieldHeight = HARD_CODED_VALUE;
            corrparams.PlayingFieldWidth = HARD_CODED_VALUE;
        }

        /// <summary>
        /// Handles the DoWork event of the <c>calibrationThread</c>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // This is also the only place an ICorrectionParams service gets created
                this.Params = new CorrectionParams();


                // Arrays for the y values of the players
                Dictionary<Player, Position> minPositions = new Dictionary<Player, Position>();
                Dictionary<Player, Position> maxPositions = new Dictionary<Player, Position>();

                //Set the PLayingFieldArea for all ObjectDetectors
                Rectangle playingFieldArea = new Rectangle(HARD_CODED_VALUE, HARD_CODED_VALUE, HARD_CODED_VALUE, HARD_CODED_VALUE);

                // TODO: Remove dependency from plugin system
                //var ownBarDetection = new Mock<IOwnBarDetection>();// ServiceLocator.LocateService<IOwnBarDetection>();
                //var opponentBarDetection = ServiceLocator.LocateService<IOpponentBarDetection>();
                //var ballDetection = ServiceLocator.LocateService<IBallDetection>();

                //if (ownBarDetection != null)
                //    ownBarDetection.PlayingFieldArea = playingFieldArea;
                //if (opponentBarDetection != null)
                //    opponentBarDetection.PlayingFieldArea = playingFieldArea;
                //if (ballDetection != null)
                //    ballDetection.PlayingFieldArea = playingFieldArea;

                //// Call the actual implementation of the calibration
                //bool result = this.DoCalibration(sender, e, minPositions, maxPositions);

                //if (!result || e.Cancel)
                //{
                //    this.State = CalibrationState.Error;
                //    return;
                //}

                //// Calculate keeper bar x position
                //int keeperBarXPosition = minPositions[Player.Keeper].XPosition;
                //keeperBarXPosition += maxPositions[Player.Keeper].XPosition;
                //keeperBarXPosition /= 2;

                //// Calculate defense bar x position
                //int defenseBarXPosition = minPositions[Player.DefenseOne].XPosition;
                //defenseBarXPosition += minPositions[Player.DefenseTwo].XPosition;
                //defenseBarXPosition += maxPositions[Player.DefenseOne].XPosition;
                //defenseBarXPosition += maxPositions[Player.DefenseTwo].XPosition;
                //defenseBarXPosition /= 4;

                //// Calculate midfield bar x position
                //int midfieldBarXPosition = minPositions[Player.MidfieldOne].XPosition;
                //midfieldBarXPosition += minPositions[Player.MidfieldTwo].XPosition;
                //midfieldBarXPosition += minPositions[Player.MidfieldThree].XPosition;
                //midfieldBarXPosition += minPositions[Player.MidfieldFour].XPosition;
                //midfieldBarXPosition += minPositions[Player.MidfieldFive].XPosition;
                //midfieldBarXPosition += maxPositions[Player.MidfieldOne].XPosition;
                //midfieldBarXPosition += maxPositions[Player.MidfieldTwo].XPosition;
                //midfieldBarXPosition += maxPositions[Player.MidfieldThree].XPosition;
                //midfieldBarXPosition += maxPositions[Player.MidfieldFour].XPosition;
                //midfieldBarXPosition += maxPositions[Player.MidfieldFive].XPosition;
                //midfieldBarXPosition /= 10;

                //// Calculate striker bar x position
                //int strikerBarXPosition = minPositions[Player.StrikerOne].XPosition;
                //strikerBarXPosition += minPositions[Player.StrikerTwo].XPosition;
                //strikerBarXPosition += minPositions[Player.StrikerThree].XPosition;
                //strikerBarXPosition += maxPositions[Player.StrikerOne].XPosition;
                //strikerBarXPosition += maxPositions[Player.StrikerTwo].XPosition;
                //strikerBarXPosition += maxPositions[Player.StrikerThree].XPosition;
                //strikerBarXPosition /= 6;

                //// Hand the calculated values to the coach
                //this.Coach.SetBarXPosition(Bar.Keeper, (ushort)keeperBarXPosition);
                //this.Coach.SetBarXPosition(Bar.Defense, (ushort)defenseBarXPosition);
                //this.Coach.SetBarXPosition(Bar.Midfield, (ushort)midfieldBarXPosition);
                //this.Coach.SetBarXPosition(Bar.Striker, (ushort)strikerBarXPosition);

                //foreach (Player player in Enum.GetValues(typeof(Player)))
                //{
                //    this.Coach.SetPlayerMinYPosition(player, (ushort)minPositions[player].YPosition);
                //    this.Coach.SetPlayerMaxYPosition(player, (ushort)maxPositions[player].YPosition);
                //}

                //// Call this additional method after coach and params have been set
                //this.AfterCalibration(sender, e);

                //// Calculate the lengths of the bars and send them to the controller
                //ushort[] barLengths = new ushort[Enum.GetValues(typeof(Bar)).Length];

                //barLengths[(int)Bar.Striker] = (ushort)(this.Coach.GetPlayerMaxYPosition(Player.StrikerOne) - this.Coach.GetPlayerMinYPosition(Player.StrikerOne));
                //this.Controller.SetBarLengthInPixel(Bar.Striker, barLengths[(int)Bar.Striker]);
                //Thread.Sleep(500);

                //barLengths[(int)Bar.Midfield] = (ushort)(this.Coach.GetPlayerMaxYPosition(Player.MidfieldOne) - this.Coach.GetPlayerMinYPosition(Player.MidfieldOne));
                //this.Controller.SetBarLengthInPixel(Bar.Midfield, barLengths[(int)Bar.Midfield]);
                //Thread.Sleep(500);

                //barLengths[(int)Bar.Defense] = (ushort)(this.Coach.GetPlayerMaxYPosition(Player.DefenseOne) - this.Coach.GetPlayerMinYPosition(Player.DefenseOne));
                //this.Controller.SetBarLengthInPixel(Bar.Defense, barLengths[(int)Bar.Defense]);
                //Thread.Sleep(500);

                //barLengths[(int)Bar.Keeper] = (ushort)(this.Coach.GetPlayerMaxYPosition(Player.Keeper) - this.Coach.GetPlayerMinYPosition(Player.Keeper));
                //this.Controller.SetBarLengthInPixel(Bar.Keeper, barLengths[(int)Bar.Keeper]);
                //Thread.Sleep(500);

                // TODO: Registration necessary ?
                // Register coach and correction parameters
                // ServiceLocator.RegisterService<ICoach>(this.Coach);
                // ServiceLocator.RegisterService<ICorrectionParams>(this.Params);

                this.State = CalibrationState.Finished;
            }
            catch (Exception ex)
            {
                //TODO: Log Exception
                this.State = CalibrationState.Error;
            }

        }
    }
}
