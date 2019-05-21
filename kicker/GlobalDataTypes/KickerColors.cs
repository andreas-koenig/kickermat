namespace GlobalDataTypes
{
    using System.Drawing;

    /// <summary>
    /// Static class to set the used colors globally.
    /// </summary>
    public static class KickerColors
    {
        /// <summary>
        /// Initializes static members of the KickerColors class.
        /// </summary>
        static KickerColors()
        {
            BallColor = Color.Yellow;
            InvalidBallColor = Color.DarkGoldenrod;
            PredictedBallColor = Color.SeaGreen;
            CorrrectedBallColor = Color.LightBlue;
            OpponentBarColor = Color.Coral;
            OwnBarColor = Color.CornflowerBlue;
            CapturedFramesColor = Color.Lime;
            ConvertedFramesColor = Color.DodgerBlue;
            GameControlColor = Color.Turquoise;
            CalibrationColor = Color.White;
        }
        
        /// <summary>
        /// Gets the color of the ball for displaying information.
        /// </summary>
        public static Color BallColor { get; private set; }

        /// <summary>
        /// Gets the color of the ball if its position is invalid.
        /// </summary>
        public static Color InvalidBallColor { get; private set; }

        /// <summary>
        /// Gets the color of the predicted position of the ball.
        /// </summary>
        public static Color PredictedBallColor { get; private set; }

        /// <summary>
        /// Gets the color of the parallax corrected position of the ball.
        /// </summary>
        public static Color CorrrectedBallColor { get; private set; }
        
        /// <summary>
        /// Gets the color of the opponent bar for displaying information.
        /// </summary>
        public static Color OpponentBarColor { get; private set; }

        /// <summary>
        /// Gets the color of the own bar for displaying information.
        /// </summary>
        public static Color OwnBarColor { get; private set; }

        /// <summary>
        /// Gets the color of the captured frames for displaying information.
        /// </summary>
        public static Color CapturedFramesColor { get; private set; }

        /// <summary>
        /// Gets the color of the converted frames for displaying information.
        /// </summary>
        public static Color ConvertedFramesColor { get; private set; }

        /// <summary>
        /// Gets the color of the game control for displaying information.
        /// </summary>
        public static Color GameControlColor { get; private set; }

        /// <summary>
        /// Gets the color of the calibratipn for displaying information.
        /// </summary>
        public static Color CalibrationColor { get; private set; }
    }
}