namespace ImageProcessing.ImageMorphology
{
    using Emgu.CV.CvEnum;
    /// <summary>
    /// Settings for morphologic operations.
    /// </summary>
    public class EmguImageMorphologySettings
    {
        /// <summary>
        /// Gets or sets the performed morphologic operation.
        /// </summary>
        /// <value>The performed morphologic operation.</value>
        public MorphOp PerformedMorphologicOperation { get; set; }

        /// <summary>
        /// Gets or sets whether Morhpology is performed.
        /// </summary>
        public bool MorphologyEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the Image should be smoothed.
        /// </summary>
        public bool PerformGaussianSmoothing { get; set; } = false;

        public BorderType BorderType { get; set; }

        public int MorphologyIterations { get; set; } = 1;

        public int SmoothKernelWidth { get; set; } = 1;

        public int SmoothKernelHeight { get; set; } = 1;

        public int SmoothSigma1 { get; set; } = 1;

        public int SmoothSigma2 { get; set; } = 1;

        public bool NoiseReductionEnabled { get; set; }

        public MorphologyMaskType MorphologyMaskType { get; set; }

        public int MorphologyMaskWidth { get; set; } = 1;

        public int MorphologyMaskHeight { get; set; } = 1;
    }

    public enum MorphologyMaskType
    {
        Rectangle
    }
}