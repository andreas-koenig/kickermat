namespace ObjectSearch
{
    using System.Drawing;
    using GlobalDataTypes;
    using PluginSystem;
    using Emgu.CV;

    /// <summary>
    /// Delegate for callbacks when a new binarized image is available.
    /// </summary>
    /// <param name="image">The new image.</param>
    public delegate void NewBinaryImageCallback(IImage image);

    /// <summary>
    /// Interface which must be implemented for a class which searches for objects in an image.
    /// </summary>
    public interface IObjectSearch : IXmlConfigurableKickerPlugin
    {
        /// <summary>
        /// Gets or sets a callback when a new binary image is available.
        /// </summary>
        NewBinaryImageCallback NewBinaryImageCallback { get; set; }

        /// <summary>
        /// Gets the number of found objects.
        /// </summary>
        /// <value>The number of found objects.</value>
        int NumberOfFoundObjects { get; }

        /// <summary>
        /// Executes an object detection run.
        /// </summary>
        void Execute(IImage image, ulong framecount);

        /// <summary>
        /// Gets the center coordinates of an object.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The object center.</returns>
        Position GetObjectCenter(int index);
        int GetBiggestObjectIndex();
        /// <summary>
        /// Gets the bounding box of an object.
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <returns>The bounding box of the object.</returns>
        Rectangle GetObjectBounds(int index);

        /// <summary>
        /// Retunrs the size of a found object.
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <returns>The size of the object.</returns>
        int ObjectSize(int index);

        /// <summary>
        /// Sorts the objects with the current sort settings.
        /// </summary>
        void SortObjects();

        Rectangle PLayingFieldArea { get; set; }

        Rectangle AreaOfInterestForNextSearch { get; set; }
    }
}