namespace DataProcessing
{
    using System;
    using System.ComponentModel;

    using Calibration;
    using Emgu.CV;
    using GameController;
    using ObjectDetection;
    using PositionPrediction;
    using Game;

    /// <summary>
    /// Klasse zur Verwaltung der Bildverarbeitungs-Module.
    /// </summary>
    public sealed class DataProcessor : IDisposable
    {
        /// <summary>
        /// Current instance for ball detection.
        /// </summary>
        private IBallDetection ballDetection;

        /// <summary>
        /// Current instance for opponent bar detection.
        /// </summary>
        private IOpponentBarDetection opponentBarDetection;

        /// <summary>
        /// Current instance for own bar detection.
        /// </summary>
        private IOwnBarDetection ownBarDetection;

        /// <summary>
        /// The background worker for execution of the game controller.
        /// </summary>
        private BackgroundWorker backgroundGameControl;

        /// <summary>
        /// The used calibration instance
        /// </summary>
        private ICalibration currentCalibration;

        /// <summary>
        /// The used distance prediction.
        /// </summary>
        private IPositionPrediction currentPositionPrediction;

        /// <summary>
        /// The currently used game controller.
        /// </summary>
        private IGameController currentGameController;

        /// <summary>
        /// Number of captured frames per second.
        /// </summary>
        private int capturedFrames;

        /// <summary>
        /// Number of captured frames from start.
        /// </summary>
        private int currentNumberOfFrames;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProcessor"/> class.
        /// </summary>
        public DataProcessor()
        {
            this.InitBackgroundWorkers();
        }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>

        /// <summary>
        /// Gets or sets the Settings.
        /// </summary>
        /// <value>The Settings.</value>
        private DataProcessorSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets the RGB image.
        /// </summary>
        /// <value>The RGB image.</value>
        private IImage RgbImage { get; set; }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (this.backgroundGameControl.IsBusy)
            {
                this.backgroundGameControl.CancelAsync();
            }

            if (this.currentCalibration.State == CalibrationState.Running)
            {
                this.currentCalibration.Cancel();
            }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute(IImage image)
        {
            this.capturedFrames++;
            this.currentNumberOfFrames++;
            this.RgbImage = image;

            this.ballDetection.Execute(this.RgbImage);
            this.ownBarDetection.Execute(this.RgbImage);
            this.opponentBarDetection.Execute(this.RgbImage);

            if (this.currentCalibration.State == CalibrationState.Finished)
            {
                if (this.backgroundGameControl.IsBusy == false)
                {
                    this.backgroundGameControl.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// Initialisiert alle verwendeten Bildverarbeitungsmodule.
        /// </summary>
        /// <param name="rgbImage">The RGB image from the video source.</param>
        public void Init(IImage rgbImage)
        {
            this.RgbImage = rgbImage;
            this.ExecuteCalibration();
        }

        /// <summary>
        /// Executes the selected calibration.
        /// </summary>
        public void ExecuteCalibration()
        {
            DialogResult result = SwissKnife.ShowQuestion(
                this,
                this.currentCalibration.GetType().Name + " selected.\n\nExecute Calibration?",
                "Calibration");

            if (result == DialogResult.Yes)
            {
                this.currentCalibration.Execute();
            }
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.Settings = SettingsSerializer.LoadSettingsFromXml<DataProcessorSettings>(xmlFileName);
            this.Settings.Validate();
        }

        /// <summary>
        /// Saves the configuration to a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void SaveConfiguration(string xmlFileName)
        {
            DataProcessorUserControl control = this.SettingsUserControl as DataProcessorUserControl;
            if (control != null)
            {
                this.Settings.SelectedTab = control.TabControlDataProcessor.SelectedIndex;
            }

            SettingsSerializer.SaveSettingsToXml(this.Settings, xmlFileName);

            // TODO: wie Dispose() über PluginSystemUserControl aufrufen
            Plugger.SavePluginSettings<ICalibration>(this.currentCalibration);
            Plugger.SavePluginSettings<IBallDetection>(this.ballDetection);
            Plugger.SavePluginSettings<IOpponentBarDetection>(this.opponentBarDetection);
            Plugger.SavePluginSettings<IOwnBarDetection>(this.ownBarDetection);
            Plugger.SavePluginSettings<IGameController>(this.currentGameController);
            Plugger.SavePluginSettings<IPositionPrediction>(this.currentPositionPrediction);
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public void InitUserControl()
        {
            DataProcessorUserControl newUserControl = new DataProcessorUserControl();

            Plugger.CreatePluginUserControl<IBallDetection>(
                this,
                this.Settings.BallDetectionAlgorithm,
                newUserControl.TabControlDataProcessor.TabPages["tabPageBallDetection"],
                this.InitBallDetection);

            Plugger.CreatePluginUserControl<IOwnBarDetection>(
                this,
                this.Settings.OwnBarDetectionAlgorithm,
                newUserControl.TabControlDataProcessor.TabPages["tabPageOwnBarDetection"],
                this.InitOwnBarDetection);

            Plugger.CreatePluginUserControl<IOpponentBarDetection>(
                this,
                this.Settings.OpponentBarDetectionAlgorithm,
                newUserControl.TabControlDataProcessor.TabPages["tabPageOpponentBarDetection"],
                this.InitOpponentBarDetection);

            Plugger.CreatePluginUserControl<ICalibration>(
                this,
                this.Settings.CalibrationAlgorithm,
                newUserControl.TabControlDataProcessor.TabPages["tabPageCalibration"],
                this.InitCalibration);

            Plugger.CreatePluginUserControl<IGameController>(
                this,
                this.Settings.GameControllerAlgorithm,
                newUserControl.TabControlDataProcessor.TabPages["tabPageGameController"],
                this.InitGameController);

            Plugger.CreatePluginUserControl<IPositionPrediction>(
                this,
                this.Settings.PositionPredictionAlgorithm,
                newUserControl.TabControlDataProcessor.TabPages["tabPagePositionPrediction"],
                this.InitPositionPrediction);

            this.SettingsUserControl = newUserControl;

            if (this.Settings.SelectedTab >= 0 &&
                this.Settings.SelectedTab < newUserControl.TabControlDataProcessor.TabCount)
            {
                newUserControl.TabControlDataProcessor.SelectedIndex = this.Settings.SelectedTab;
            }
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {
            if (this.SettingsUserControl != null)
            {
                this.SettingsUserControl.Dispose();
                this.SettingsUserControl = null;
            }

            if (this.backgroundGameControl != null)
            {
                this.backgroundGameControl.Dispose();
                this.backgroundGameControl = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Inits the calibration.
        /// </summary>
        /// <param name="newCalibration">The new calibration.</param>
        internal void InitCalibration(ICalibration newCalibration)
        {
            this.Settings.CalibrationAlgorithm = newCalibration.GetType();
            this.currentCalibration = newCalibration;
        }

        /// <summary>
        /// Inits the ball detection.
        /// </summary>
        /// <param name="newBallDetection">The new ball detection.</param>
        internal void InitBallDetection(IBallDetection newBallDetection)
        {
            this.Settings.BallDetectionAlgorithm = newBallDetection.GetType();
            this.ballDetection = newBallDetection;
        }

        /// <summary>
        /// Inits the opponent bar detection.
        /// </summary>
        /// <param name="newOpponenBarDetecion">The new opponen bar detecion.</param>
        internal void InitOpponentBarDetection(IOpponentBarDetection newOpponenBarDetecion)
        {
            this.Settings.OpponentBarDetectionAlgorithm = newOpponenBarDetecion.GetType();
            this.opponentBarDetection = newOpponenBarDetecion;
        }

        /// <summary>
        /// Inits the own bar detection.
        /// </summary>
        /// <param name="newOwnBarDetecion">The new own bar detecion.</param>
        internal void InitOwnBarDetection(IOwnBarDetection newOwnBarDetecion)
        {
            this.Settings.OwnBarDetectionAlgorithm = newOwnBarDetecion.GetType();
            this.ownBarDetection = newOwnBarDetecion;
        }

        /// <summary>
        /// Inits the game controller.
        /// </summary>
        /// <param name="newGameController">The new game controller.</param>
        internal void InitGameController(IGameController newGameController)
        {
            this.Settings.GameControllerAlgorithm = newGameController.GetType();
            this.currentGameController = newGameController;
        }

        /// <summary>
        /// Inits the position prediction.
        /// </summary>
        /// <param name="newPositionPrediction">The new position prediction.</param>
        internal void InitPositionPrediction(IPositionPrediction newPositionPrediction)
        {
            this.Settings.PositionPredictionAlgorithm = newPositionPrediction.GetType();
            this.currentPositionPrediction = newPositionPrediction;
        }

        /// <summary>
        /// Inits the background workers.
        /// </summary>
        private void InitBackgroundWorkers()
        {
            this.backgroundGameControl = new BackgroundWorker();
            this.backgroundGameControl.WorkerSupportsCancellation = true;
            this.backgroundGameControl.DoWork += this.BackgroundGameControl_DoWork;
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundGameControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void BackgroundGameControl_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Game.IsGameRunning)
                {
                    var ballpos = this.ballDetection.BallPosition.Clone();
                    SwissKnife.ParallaxCorrection(currentCalibration.Params.CameraZDistance, currentCalibration.Params.PlayerZDistance, currentCalibration.Params.PlayingFieldCenter, ballpos);
                    this.currentPositionPrediction.Run(ballpos, this.currentNumberOfFrames,ballDetection.PlayingFieldArea);
                    var predictedBallpos = currentPositionPrediction.PredictedPosition.Clone();
                    predictedBallpos.InPlayingArea = ballpos.InPlayingArea;
                    predictedBallpos.Valid = ballpos.Valid;
                    this.currentGameController.Run(predictedBallpos);
                }
            }
            catch (Exception ex)
            {
                //TODO: Log Exception
            }
        }
    }
}
