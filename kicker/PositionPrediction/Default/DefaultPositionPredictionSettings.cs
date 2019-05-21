namespace PositionPrediction.Default
{
    /// <summary>
    /// Class for storing the Settings of the default position prediction.
    /// </summary>
    public class DefaultPositionPredictionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPositionPredictionSettings"/> class.
        /// </summary>
        public DefaultPositionPredictionSettings()
        {
            this.FramesToPredict = 2;           
        }

        /// <summary>
        /// Gets or sets a value indicating whether [prediction enabled].
        /// </summary>
        /// <value><c>true</c> if [prediction enabled]; otherwise, <c>false</c>.</value>
        public bool PredictionEnabled { get; set; }

        /// <summary>
        /// Gets or sets the frames to predict.
        /// </summary>
        /// <value>The frames to predict.</value>
        public int FramesToPredict { get; set; }

        /// <summary>
        /// Gets or sets the maximum difference.
        /// </summary>
        /// <value>The maximum difference.</value>
        public int MaximumDifference { get; set; }
    }
}