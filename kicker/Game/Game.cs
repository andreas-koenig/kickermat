namespace GameProperties
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using static BarType;

    public class Game
    {
        public Game()
        {
            OwnBars = new Bars();
            OpponentBars = new Bars();
            BallPosition = new Position();
        }

        public Bars OwnBars { get; set; }

        public Bars OpponentBars { get; set; }

        public PlayingField PlayingField { get; set; }

        public Position BallPosition { get; set; }

        public bool IsRunning { get; internal set; }

        public void Init(PlayingField playingField)
        {
            PlayingField = playingField;
        }
    }

    public class PlayingField
    {
        public PlayingField(double width, double length, Point origin)
        {
            Width = width;
            Length = length;
            Origin = origin;
        }

        /// <summary>
        /// Gets the dimensions of the playing field.
        /// </summary>
        /// <param name="width">The width of the playing field.</param>
        /// <param name="height">The height of the playing field.</param>
        public double Width { get; internal set; }

        public double Length { get; internal set; }

        /// <summary>
        /// Gets the position of the origin of the playing field.
        /// </summary>
        public Point Origin { get; internal set; }

        // NOTE: This property is used by the legacy GameController
        // TODO: Only parts of the class "Position" are used ....

        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        public Position PlayingFieldOffset { get; internal set; }
    }
}
