using System;
using System.Drawing;

namespace GameProperties
{
    public class Position
    {
        private Position position;

        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool Valid { get; set; }
        public bool InPlayingArea { get; set; }
        public Rectangle BoundingBox { get; }

        public Position()
        {
            this.XPosition = 0;
            this.YPosition = 0;
            this.Valid = true;
            this.InPlayingArea = true;
        }
        public Position(int xpos, int ypos, bool valid, bool inPlayingArea)
        {
            this.XPosition = xpos;
            this.YPosition = ypos;
            this.Valid = valid;
            this.InPlayingArea = inPlayingArea;
        }

        public Position(Position position)
        {
            this.position = position;
        }

        public Position Clone()
        {
            return new Position(this);
        }
    }
}
