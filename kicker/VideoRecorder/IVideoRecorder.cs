namespace VideoRecorder
{
    using System;
    using PluginSystem;
    using Emgu.CV;

    /// <summary>
    /// Describes the interface which must be implemented by a video recorder
    /// </summary>
    public interface IVideoRecorder : IXmlConfigurableKickerPlugin, IDisposable
    {
        /// <summary>
        /// Occurs when the state state of the video recorder changed.
        /// </summary>
        event EventHandler<VideoRecorderStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Inits the specified instance.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        void Init(IImage sourceImage);

        /// <summary>
        /// Inits the recording.
        /// </summary>
        void InitRecording();

        /// <summary>
        /// Starts the recording.
        /// </summary>
        void StartRecording();

        /// <summary>
        /// Stops the recording.
        /// </summary>
        void StopRecording();

        bool Recording { get; }
    }
}