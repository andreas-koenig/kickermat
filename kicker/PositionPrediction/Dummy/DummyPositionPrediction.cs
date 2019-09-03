namespace PositionPrediction.Dummy
{
    using System.Drawing;
    using Game;

    /// <summary>
    /// Implements the default position prediction which doesn't predict any values.
    /// </summary>
    public class DummyPositionPrediction : IPositionPrediction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyPositionPrediction"/> class.
        /// </summary>
        public DummyPositionPrediction()
        {
            this.PredictedPosition = new Position();
        }

        /// <summary>
        /// Gets the predicted position.
        /// </summary>
        /// <value>The predicted position.</value>
        public Position PredictedPosition { get; private set; }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>

        /// <summary>
        /// Executes the position prediction
        /// </summary>
        /// <param name="ballPosition">The ball position.</param>
        /// <param name="numberOfFrames">The number of frames.</param>
        public void Run(Position ballPosition, int numberOfFrames, Rectangle playingArea = new Rectangle())
        {
            this.PredictedPosition.valid = ballPosition.valid;
            this.PredictedPosition.xPosition = ballPosition.xPosition;
            this.PredictedPosition.yPosition = ballPosition.yPosition;
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            // Nothing to do
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void SaveConfiguration(string xmlFileName)
        {
            // Nothing to do
        }
    }
}
