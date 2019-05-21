using System.Drawing;

namespace ObjectSearch.BlobSearch
{
    /// <summary>
    /// Class for storing blob-related information (size, position, ...)
    /// </summary>
    public class BlobData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobData"/> class.
        /// </summary>
        /// <param name="blobCenterX">The BLOB center X.</param>
        /// <param name="blobCenterY">The BLOB center Y.</param>
        /// <param name="blobSize">Size of the BLOB.</param>
        public BlobData(int blobCenterX, int blobCenterY, int blobSize, Rectangle rect)
        {
            this.BlobCenterX = blobCenterX;
            this.BlobCenterY = blobCenterY;
            this.BlobSize = blobSize;
            this.Rectangle = rect;
        }

        /// <summary>
        /// Gets or sets the size of the BLOB.
        /// </summary>
        /// <value>The size of the BLOB.</value>
        public int BlobSize { get; set; }

        /// <summary>
        /// Gets or sets the BLOB center X.
        /// </summary>
        /// <value>The BLOB center X.</value>
        public int BlobCenterX { get; set; }

        /// <summary>
        /// Gets or sets the BLOB center Y.
        /// </summary>
        /// <value>The BLOB center Y.</value>
        public int BlobCenterY { get; set; }

        public Rectangle Rectangle { get; set; } 
    }
}