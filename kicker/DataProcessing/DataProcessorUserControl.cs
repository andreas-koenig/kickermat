namespace DataProcessing
{
    using System.Windows.Forms;
    using GameController;
    using ObjectDetection;
    using PluginSystem;
    using PositionPrediction;

    /// <summary>
    /// User Interface für die Bildverarbeitung
    /// </summary>
    public partial class DataProcessorUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProcessorUserControl"/> class.
        /// </summary>
        public DataProcessorUserControl()
        {            
            this.InitializeComponent();            
        }

        /// <summary>
        /// Gets the tab control data processor.
        /// </summary>
        /// <value>The tab control data processor.</value>
        public TabControl TabControlDataProcessor
        {
            get
            {
                return this.tabControlDataProcessor;
            }
        }              

        /// <summary>
        /// Gets or sets the game controller user control.
        /// </summary>
        /// <value>The game controller user control.</value>
        private PluginSystemUserControl<IGameController> GameControllerUserControl { get; set; }
     
        /// <summary>
        /// Gets or sets the ball detection user control.
        /// </summary>
        /// <value>The ball detection user control.</value>
        private PluginSystemUserControl<IBallDetection> BallDetectionUserControl { get; set; }
     
        /// <summary>
        /// Gets or sets the own bar detection user control.
        /// </summary>
        /// <value>The own bar detection user control.</value>
        private PluginSystemUserControl<IOwnBarDetection> OwnBarDetectionUserControl { get; set; }
      
        /// <summary>
        /// Gets or sets the opponent bar detection user control.
        /// </summary>
        /// <value>The opponent bar detection user control.</value>
        private PluginSystemUserControl<IOpponentBarDetection> OpponentBarDetectionUserControl { get; set; }
       
        /// <summary>
        /// Gets or sets the position prediction user control.
        /// </summary>
        /// <value>The position prediction user control.</value>
        private PluginSystemUserControl<IPositionPrediction> PositionPredictionUserControl { get; set; }                 
    }
}