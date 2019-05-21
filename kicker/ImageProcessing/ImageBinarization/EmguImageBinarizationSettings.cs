namespace ImageProcessing.ImageBinarization
{
    /// <summary>
    /// Settings for a image binarization instance.
    /// </summary>
    public class EmguImageBinarizationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmguImageBinarizationSettings"/> class.
        /// </summary>
        public EmguImageBinarizationSettings()
        {
            // Create default values, used if deserialization failed
            this.PlaneCount = 3;
            this.ColorMinValues = new[] { 0, 0, 0 };
            this.ColorMaxValues = new[] { 255, 255, 255 };
            this.BinarizePlane = new[] { true, true, true };
            this.InvertPlane = new[] { false, false, false };
        }

        /// <summary>
        /// Gets or sets the plane count.
        /// </summary>
        /// <value>The plane count.</value>
        public int PlaneCount { get; set; }

        /// <summary>
        /// Gets or sets the color min values.
        /// </summary>
        /// <value>The color min values.</value>
        public int[] ColorMinValues { get; set; }

        /// <summary>
        /// Gets or sets the color max values.
        /// </summary>
        /// <value>The color max values.</value>
        public int[] ColorMaxValues { get; set; }

        /// <summary>
        /// Gets or sets the values which indicate if a plane is binarized or not.
        /// </summary>
        /// <value>The values which indicate if a plane is binarized or not.</value>
        public bool[] BinarizePlane { get; set; }

        public bool[] InvertPlane { get; set; }
    }
}