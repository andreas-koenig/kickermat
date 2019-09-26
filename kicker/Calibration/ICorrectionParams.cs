namespace Calibration
{
    using GameProperties;
    /// <summary>
    /// Interface to an instance containing playing field parameters after correction.
    /// </summary>
    public interface ICorrectionParams
    {
        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        Position PlayingFieldOffset { get; }

        /// <summary>
        /// Gets the position of the center of the playing field.
        /// </summary>
        Position PlayingFieldCenter { get; }

        /// <summary>
        /// Gets the width of the playing field.
        /// </summary>
        int PlayingFieldWidth { get; }

        /// <summary>
        /// Gets the height of the playing field.
        /// </summary>
        int PlayingFieldHeight { get; }

        /// <summary>
        /// Gets the distance of the camera above the playing field.
        /// </summary>
        int CameraZDistance { get; }

        /// <summary>
        /// Gets the distance of the players heads above the playing field.
        /// </summary>
        int PlayerZDistance { get; }

        /// <summary>
        /// Gets the diameter of the ball.
        /// </summary>
        int BallDiameter { get; }
    }
}
