namespace ObjectDetection
{
    using GlobalDataTypes;
    using PluginSystem;

    /// <summary>
    /// Default algorithm for ball detection.
    /// </summary>
    public sealed class DefaultBallDetection : BasicObjectDetection<DefaultBallDetectionSettings>, IBallDetection
    {
        /// <summary>
        /// Stores the frame counter of the last frame to check if a execution is older than an previous one.
        /// </summary>
        private ulong lastFrameCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBallDetection"/> class.
        /// </summary>
        public DefaultBallDetection()
            : base(4)
        {
            this.BallPosition = new Position();

            ServiceLocator.RegisterService<IBallDetection>(this);
        }

        /// <summary>
        /// Gets the ball position.
        /// </summary>
        /// <value>The ball position.</value>
        public Position BallPosition { get; private set; }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public override void Dispose()
        { 
            base.Dispose();
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public override void InitUserControl()
        {
            base.InitUserControl();
        }

        /// <summary>
        /// Detects the objects.
        /// </summary>
        /// <param name="frameCounter">The frame counter.</param>
        protected override void DetectObjects(ulong frameCounter)
        {
            // Check if the frame to process is out of date
            if (frameCounter > this.lastFrameCounter)
            {
                // If objects have been found, the first has to be the ball
                if (this.ObjectSearch.NumberOfFoundObjects > 0)
                {
                    this.BallPosition = this.ObjectSearch.GetObjectCenter(this.ObjectSearch.GetBiggestObjectIndex());
                    this.BallPosition.Valid = true;
                    //Check whether ball is within the PLayingArea
                    if (PlayingFieldArea.X <= BallPosition.XPosition && (PlayingFieldArea.X + PlayingFieldArea.Width) >= BallPosition.XPosition &&
                        PlayingFieldArea.Y <= BallPosition.YPosition && (PlayingFieldArea.Y + PlayingFieldArea.Height) >= BallPosition.YPosition)
                        this.BallPosition.InPlayingArea = true;
                    else
                        this.BallPosition.InPlayingArea = false;
                }
                else
                {
                    this.BallPosition.Valid = false;
                }

                this.lastFrameCounter = frameCounter;
            }
        }
    }
}