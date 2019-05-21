namespace ObjectDetection
{
    using Emgu.CV;
    using ObjectSearch;
    using PluginSystem;
    using System.Drawing;

    /// <summary>
    /// Describes the interface of an object detection algorithm.
    /// </summary>
    public interface IObjectDetection : IXmlConfigurableKickerPlugin
    {
        /// <summary>
        /// Gets the object search.
        /// </summary>
        IObjectSearch ObjectSearch { get; }

        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>
        int ExecutionCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        void Execute(IImage image);

        Rectangle PlayingFieldArea { get; set; }

        Rectangle AreaOfInterestForNextSearch { get; set; }
    }
}