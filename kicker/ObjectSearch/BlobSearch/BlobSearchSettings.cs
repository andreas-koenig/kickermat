namespace ObjectSearch.BlobSearch
{
    using System.Xml.Serialization;
    using Emgu.CV.Features2D;
    using Emgu.CV.Structure;
    using ObjectSearch.Base;
    using Utilities;

    /// <summary>
    /// Contains all Settings which can set done for blob detection
    /// </summary>
    public class BlobSearchSettings : BasicObjectSearchSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobSearchSettings"/> class.
        /// </summary>
        public BlobSearchSettings()
        {
        }

        public int MinBlobArea { get; set; }

        public int MaxBlobArea { get; set; }

        /// <summary>
        /// Gets or sets the distance to search next BLOB.
        /// </summary>
        /// <value>The distance to search next BLOB.</value>
        public int DistanceToSearchNextBlob { get; set; }

        public int MinimumNumberOfObjectsToFind { get; set; }

        public int AreaOfInterestWidth { get; set; } = 50;
        public int AreaOfInterestHeight { get; set; } = 50;
    }
}