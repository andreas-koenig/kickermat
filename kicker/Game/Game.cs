namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using static BarType;

    public class Game
    {
        public Bars ownBars;
        public Bars opponentBars;
        public PlayingField playingField;

        public CalibrationState State { get; protected set; }

        private Boolean isRunning;
        public Boolean IsRunning
        {
            get
            {
                return isRunning;
            }
            internal set
            {
                isRunning = value;
            }
        }

        public Game()
        {
            ownBars = new Bars();
            opponentBars = new Bars();
            ballPosition = new Position();
        }

        public Position ballPosition;

        public void Init(PlayingField playingField)
        {
            this.playingField = playingField;
        }
    }

    public class PlayingField
    {
        /// <summary>
        /// Sets the dimensions of the playing field.
        /// </summary>
        /// <param name="width">The width of the playing field.</param>
        /// <param name="height">The height of the playing field.</param>
        public double Width { get; private set; }
        public double Length { get; private set; }

        /// <summary>
        /// Gets the position of the origin of the playing field.
        /// </summary>
        public Point Origin { get; private set; }

        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        //public Position PlayingFieldOffset { get; private set; }
        public PlayingField(double width, double length, Point origin)
        {
            this.Width = width;
            this.Length = length;
            this.Origin = origin;
        }
    }
}
