using System.Drawing;

namespace Game
{
    public class Position
    {
        public int xPosition { get; set; }

        public int yPosition { get; set; }
        public bool valid { get; set; }
        public bool inPlayingArea { get; set; }
        public Rectangle boundingBox { get; }

        public Position()
        {
        }

        //TODO: Remove?
        public Position(Point point, bool valid, bool inPlayingArea, Rectangle boundingBox)
        {
            this.xPosition = point.X;
            this.yPosition = point.Y;
            this.valid = valid;
            this.inPlayingArea = inPlayingArea;
            this.boundingBox = boundingBox;
        }

        public Position(int xpos, int ypos, bool valid, bool inPlayingArea)
        {
            this.xPosition = xpos;
            this.yPosition = ypos;
            this.valid = valid;
            this.inPlayingArea = inPlayingArea;
        }
    }
}
