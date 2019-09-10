namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using static BarType;

    public class Game
    {
        private Bars ownBars;
        private Bars opponentBars;
        private Position ballPosition;

        public CalibrationState State { get; protected set; }

        private Boolean isRunning;
        public Boolean IsRunning
        {
            get
            {
                return IsRunning;
            }
            internal set
            {
                isRunning = value;
            }
        }

        public void Init()
        {
            ownBars = new Bars();
            opponentBars = new Bars();
        }
    }

    public class PlayingField
    {
        public int playingFieldWidth { get; private set; }
        public int playingFieldLength { get; private set; }

        /// <summary>
        /// Sets the dimensions of the playing field.
        /// </summary>
        /// <param name="width">The width of the playing field.</param>
        /// <param name="height">The height of the playing field.</param>
        public void SetFieldDimensions(int width, int length)
        {
            this.playingFieldWidth = width;
            this.playingFieldLength = length;
        }

        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        //public Position PlayingFieldOffset { get; private set; }

        /// <summary>
        /// Gets the position of the center of the playing field.
        /// </summary>
        public Point PlayingFieldCenter { get; private set; }
    }
}
