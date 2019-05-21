namespace ObjectDetection
{
    using ObjectSearch;
    using PluginSystem;

    /// <summary>
    /// Describes the interface of the basic object detection algorithm.
    /// </summary>
    internal interface IBasicObjectDetection : IObjectDetection
    {
        /// <summary>
        /// Gets the settings of this instance.
        /// </summary>
        BasicObjectDetectionSettings Settings { get; }
    }
}