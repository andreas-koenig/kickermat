using System.Drawing;

namespace Game
{
    public class Position
    {
        Point point { get; }
        public bool valid { get; set; }
        public bool inPlayingArea { get; set; }
        public Rectangle boundingBox { get; }
        public Position(Point point, bool valid, bool inPlayingArea, Rectangle boundingBox)
        {
            this.point = point;
            this.valid = valid;
            this.inPlayingArea = inPlayingArea;
            this.boundingBox = boundingBox;
        }
    }
}