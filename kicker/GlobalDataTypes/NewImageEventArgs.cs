using Emgu.CV;

namespace GlobalDataTypes
{
    using System;

    /// <summary>
    /// Event-Parameter für ein NewImage-Event der Videoquelle.
    /// </summary>
    public class NewImageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewImageEventArgs"/> class.
        /// </summary>
        /// <param name="newImage">The new image.</param>
        public NewImageEventArgs(IImage newImage)
        {
            this.Image = newImage;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>The image.</value>
        public IImage Image { get; private set; }
    }
}