namespace ObjectSearchPreparation.RgbToBinary
{
    using Base;
    using ImageProcessing.ImageBinarization;
    using ImageProcessing.ImageMorphology;

    /// <summary>
    /// Class for converting a RGB image to a binary image.
    /// </summary>
    public sealed class RgbToBinary : BasicObjectSearchPreparation<RgbToBinarySettings>
    {
        /// <summary>
        /// Inits the image processor list.
        /// </summary>
        protected override void InitImageProcessorList()
        {
            EmguImageBinarization imageBinarization = new EmguImageBinarization(
                new[] { "Red", "Green", "Blue" },
                this.Settings.SettingsForCvbImageBinarization);
            this.ImageProcessors.Add(imageBinarization);

            EmguImageMorphology imageMorphology = new EmguImageMorphology(this.Settings.SettingsForCvbImageMorphology);
            this.ImageProcessors.Add(imageMorphology);
        }
    }
}
