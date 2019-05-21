namespace VideoRecorder
{
    /// <summary>
    /// All operation states of a video recorder.
    /// </summary>
    public enum VideoRecorderState
    {
        /// <summary>
        /// Video recorder has loaded a new image successfully.
        /// </summary>
        ImageLoaded,

        /// <summary>
        /// Recording was initialized by the instance.
        /// </summary>
        RecordingInitialized,

        /// <summary>
        /// Recording was started by the instance.
        /// </summary>
        RecordingStarted,
        
        /// <summary>
        /// Recording was stopped by the instance.
        /// </summary>
        RecordingStopped
    }
}
