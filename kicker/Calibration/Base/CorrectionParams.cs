namespace Calibration.Base
{
    using GameProperties;

    /// <summary>
    /// This class contains playing field parameters after offset and parallax correction.
    /// </summary>
    public class CorrectionParams : ICorrectionParams
    {
        /// <summary>
        /// Initializes a new instance of the CorrectionParams class.
        /// </summary>
        public CorrectionParams()
        {
            this.PlayingFieldOffset = new Position(0, 0, false, false);
            this.PlayingFieldCenter = new Position(0, 0, false, false);
        }

        /// <summary>
        /// Gets or sets  the position of the top left cornder of the playing field.
        /// </summary>
        public Position PlayingFieldOffset { get; internal set; }

        /// <summary>
        /// Gets or sets  the position of the center of the playing field.
        /// </summary>
        public Position PlayingFieldCenter { get; internal set; }

        /// <summary>
        /// Gets or sets  the distance of the camera above the playing field.
        /// </summary>
        public int CameraZDistance { get; internal set; }

        /// <summary>
        /// Gets or sets  the distance of the players heads above the playing field.
        /// </summary>
        public int PlayerZDistance { get; internal set; }

        /// <summary>
        /// Gets or sets  the diameter of the ball.
        /// </summary>
        public int BallDiameter { get; internal set; }

        /// <summary>
        /// Gets or sets  the width of the playing field.
        /// </summary>
        public int PlayingFieldWidth { get; internal set; }

        /// <summary>
        /// Gets or sets the height of the playing field.
        /// </summary>
        public int PlayingFieldHeight { get; internal set; }
    }
}
