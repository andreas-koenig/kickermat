namespace VideoSource
{
    using System;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using GlobalDataTypes;
    using PluginSystem;
    using System.Drawing;

    /// <summary>
    /// Interface for a video source implementation.
    /// </summary>
    public interface IVideoSource : IXmlConfigurableKickerPlugin, IDisposable
    {
        /// <summary>
        /// Occurs when a new image is aquired.
        /// </summary>
        event EventHandler<NewImageEventArgs> NewImage;

        /// <summary>
        /// Gets the raw image.
        /// </summary>
        /// <value>The raw image.</value>
        IImage RawImage { get; }

        /// <summary>
        /// Gets the RGB image.
        /// </summary>
        /// <value>The RGB image.</value>
        IImage RgbImage { get; }

        /// <summary>
        /// Loads the image from the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if loading was successful, else false.</returns>
        bool LoadVideoSource(string fileName);

        /// <summary>
        /// Gets a new image.
        /// </summary>
        void GetNewImage();

        /// <summary>
        /// Starts the acquisition.
        /// </summary>
        void StartAcquisition();

        /// <summary>
        /// Stops the acquisition.
        /// </summary>
        void StopAcquisition();

        /// <summary>
        /// Gets whether the VideoSource is capturing Images
        /// </summary>
        bool Acquiring { get; }
    }
}