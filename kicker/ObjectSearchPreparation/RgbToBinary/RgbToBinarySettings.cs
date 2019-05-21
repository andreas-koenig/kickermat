namespace ObjectSearchPreparation.RgbToBinary
{
    using ImageProcessing.ImageBinarization;
    using ImageProcessing.ImageMorphology;

    /// <summary>
    /// Class for storing the settings which are used to convert a RGB image to a binary image.
    /// </summary>
    public sealed class RgbToBinarySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RgbToBinarySettings"/> class.
        /// </summary>
        public RgbToBinarySettings()
        {
            // Create default instances because if no valid configuration is available, 
            // a null pointer exception is thrown
            this.SettingsForCvbImageBinarization = new EmguImageBinarizationSettings();
            this.SettingsForCvbImageMorphology = new EmguImageMorphologySettings();
        }

        /// <summary>
        /// Gets or sets the settings for CVB image binarization.
        /// </summary>
        /// <value>The settings for CVB image binarization.</value>
        public EmguImageBinarizationSettings SettingsForCvbImageBinarization { get; set; }

        /// <summary>
        /// Gets or sets the settings for CVB image morphology.
        /// </summary>
        /// <value>The settings for CVB image morphology.</value>
        public EmguImageMorphologySettings SettingsForCvbImageMorphology { get; set; }
    }
}
