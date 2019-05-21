using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;

namespace ImageProcessing
{

    /// <summary>
    /// Interface for an image processing component.
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Gets the Settings user control.
        /// </summary>
        /// <value>The Settings user control.</value>
        UserControl SettingsUserControl { get; }

        /// <summary>
        /// Executes the operation on the specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <returns>The result image.</returns>
        IImage Execute(IImage sourceImage);
    }
}