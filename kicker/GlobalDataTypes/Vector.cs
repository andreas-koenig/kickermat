namespace GlobalDataTypes
{
    using System;

    /// <summary>
    /// Vector needed for direction
    /// </summary>
    public class Vector : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        public Vector()
        {
            this.XPosition = 0;
            this.YPosition = 0;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">The X-Direction</param>
        /// <param name="y">The Y-Direction.</param>
        public Vector(int x, int y)
        {
            this.XPosition = x;
            this.YPosition = y;
        }

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
        
        /// <summary>
        /// Erstellt ein neues Objekt, das eine Kopie der aktuellen Instanz darstellt.
        /// </summary>
        /// <returns>
        /// Ein neues Objekt, das eine Kopie dieser Instanz darstellt.
        /// </returns>
        public object Clone()
        {
            Vector vector = new Vector(this.XPosition, this.YPosition);
            return vector;
        }
    }
}