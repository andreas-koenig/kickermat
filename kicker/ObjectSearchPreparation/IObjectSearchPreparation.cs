namespace ObjectSearchPreparation
{
    using Emgu.CV;
    using PluginSystem;
    using System.Drawing;

    /// <summary>
    /// Interface which must be implemented by a class which implementes an image binarization.
    /// </summary>
    public interface IObjectSearchPreparation : IXmlConfigurableKickerPlugin
    {
        /// <summary>
        /// Executes the object search preparation with specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <returns>The resulting binary image.</returns>
        IImage Execute(IImage sourceImage);
    }
}
