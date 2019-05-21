namespace ObjectDetection
{
    using GlobalDataTypes;
    using PluginSystem;

    /// <summary>
    /// Interface which must be implemented by a ball detection algorithm.
    /// </summary>
    public interface IBallDetection : IObjectDetection
    {
        /// <summary>
        /// Gets the ball position.
        /// </summary>
        /// <value>The ball position.</value>
        Position BallPosition { get; }
    }
}