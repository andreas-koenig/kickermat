using System;
using System.Drawing;

namespace GameProperties
{
    public class Position
    {
        private Position _position;

        public Position()
        {
            XPosition = 0;
            YPosition = 0;
            Valid = true;
            InPlayingArea = true;
        }

        public Position(int xpos, int ypos, bool valid, bool inPlayingArea)
        {
            XPosition = xpos;
            YPosition = ypos;
            Valid = valid;
            InPlayingArea = inPlayingArea;
        }

        public Position(Position position)
        {
            _position = position;
        }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public bool Valid { get; set; }

        public bool InPlayingArea { get; set; }

        public Rectangle BoundingBox { get; }

        public Position Clone()
        {
            return new Position(this);
        }
    }
}
