namespace PositionPrediction
{
    using System;

    /// <summary>
    /// the predicted Ballposition
    /// </summary>
    internal sealed class PredictedBallPosition : Position, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PredictedBallPosition"/> class.
        /// </summary>
        public PredictedBallPosition()
            : base(0, 0, false,false)
        {
            this.FrameNumber = 0;
            this.Direction = new Vector(0, 0);
            this.FrameDifference = 0;
        }

        /// <summary>
        /// Gets or sets the frame number (absolut frame number from start of camera).
        /// </summary>
        public int FrameNumber { get; set; }
        
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public Vector Direction { get; set; }

        /// <summary>
        /// Gets or sets the frame difference between two frames.
        /// </summary>
        public int FrameDifference { get; set; }

        /// <summary>
        /// Sets the values Ballposition, FrameNumber and FrameDifference.
        /// </summary>
        /// <param name="pos">The ballposition.</param>
        /// <param name="frameNumber">The frame number.</param>
        /// <param name="frameDifference">The frame difference.</param>
        public void SetValues(Position pos, int frameNumber, int frameDifference)
        {
            this.XPosition = pos.XPosition;
            this.YPosition = pos.YPosition;
            this.Valid = pos.Valid;
            this.FrameDifference = frameDifference;
            this.FrameNumber = frameNumber;
            this.InPlayingArea = pos.InPlayingArea;
        }

        /// <summary>
        /// Resets the values.
        /// </summary>
        /// <param name="pos">The ballposition.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="frameDifference">The frame difference.</param>
        public void ResetValues(Position pos, Vector direction, int frameDifference)
        {
            this.XPosition = pos.XPosition;
            this.YPosition = pos.YPosition;
            this.Valid = pos.Valid;
            this.FrameDifference = frameDifference;
            this.Direction = (Vector)direction.Clone();
        }

        /// <summary>
        /// Erstellt ein neues Objekt, das eine Kopie der aktuellen Instanz darstellt.
        /// </summary>
        /// <returns>Ein neues Objekt, das eine Kopie dieser Instanz darstellt.</returns>
        public new object Clone()
        {
            PredictedBallPosition ballPosition = new PredictedBallPosition();
            ballPosition.Direction = (Vector)this.Direction.Clone();
            ballPosition.FrameDifference = this.FrameDifference;
            ballPosition.FrameNumber = this.FrameNumber;
            ballPosition.Valid = this.Valid;
            ballPosition.XPosition = this.XPosition;
            ballPosition.YPosition = this.YPosition;
            ballPosition.InPlayingArea = this.InPlayingArea;
            return ballPosition;
        }
    }
}
