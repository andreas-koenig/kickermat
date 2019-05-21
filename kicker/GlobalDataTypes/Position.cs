using System.Drawing;

namespace GlobalDataTypes
{
    /// <summary>
    /// Class for storing positions of the object detection.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        public Position()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="other">Another position object to take the values from.</param>
        public Position(Position other)
        {
            this.XPosition = other.XPosition;
            this.YPosition = other.YPosition;
            this.Valid = other.Valid;
            this.BoundingBox = other.BoundingBox;
            this.InPlayingArea = other.InPlayingArea;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="xpos">The x position.</param>
        /// <param name="ypos">The y position.</param>
        /// <param name="valid">if set to <c>true</c> [position valid].</param>
        public Position(int xpos, int ypos, bool valid, bool inPlayingArea)
        {
            this.XPosition = xpos;
            this.YPosition = ypos;
            this.Valid = valid;
            this.InPlayingArea = InPlayingArea;
        }

        public Position(int xpos, int ypos, bool valid, bool inPlayingArea, Rectangle boundingBox) : this(
            xpos, ypos, valid, inPlayingArea)
        {
            BoundingBox = boundingBox;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Position"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the X position.
        /// </summary>
        /// <value>The X position.</value>
        public int XPosition { get; set; }

        /// <summary>
        /// Gets or sets the Y position.
        /// </summary>
        /// <value>The Y position.</value>
        public int YPosition { get; set; }

        public Rectangle BoundingBox { get; set; }

        public bool InPlayingArea { get; set; }

        public Position Clone()
        {
            return new Position(this);
        }
    }
}