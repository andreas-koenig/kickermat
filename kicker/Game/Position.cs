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
        public Position(int xpos, int ypos, bool valid, bool inPlayingArea)
        {
            this.xPosition = xpos;
            this.yPosition = ypos;
            this.valid = valid;
            this.inPlayingArea = inPlayingArea;
        }
    }
}
