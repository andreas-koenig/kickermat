namespace VideoRecorder
{
    using System;

    /// <summary>
    /// Event arguments for a video recorder's state changed event.
    /// </summary>
    public sealed class VideoRecorderStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoRecorderStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newState">The new state.</param>
        public VideoRecorderStateChangedEventArgs(VideoRecorderState newState)
        {
            this.NewState = newState;
        }

        /// <summary>
        /// Gets the new state.
        /// </summary>
        /// <value>The new state.</value>
        public VideoRecorderState NewState { get; private set; }
    }
}
