namespace PositionPrediction
{
    using GlobalDataTypes;
    using System.Drawing;

    /// <summary>
    /// Interface which describes the distance prediction module
    /// </summary>
    public interface IPositionPrediction
    {
        /// <summary>
        /// Gets the predicted position.
        /// </summary>
        /// <value>The predicted position.</value>
        Position PredictedPosition { get; }

        /// <summary>
        /// Executes a distance prediction
        /// </summary>
        /// <param name="ballPosition">The ball position.</param>
        /// <param name="frameNumber">The frame number.</param>
        void Run(Position ballPosition, int frameNumber, Rectangle playingArea = new Rectangle());
    }
}