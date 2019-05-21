namespace VideoRecorder
{
    /// <summary>
    /// Acquisition modes of a video recorder.
    /// </summary>
    public enum VideoRecorderAcquisitionMode
    {
        /// <summary>
        /// In this mode, CVB Movie acquires each image by calling the Snap function on this image object. 
        /// This method is equivalent to grabbing images with PingPong disabled.
        /// </summary>
        SingleSnap = 0x00,

        /// <summary>
        /// In this mode, CVB Movie acquires the images, using the image object's IPingPong interface.
        /// This is potentially the fastest way, because acquisition is taking place in a separate 
        /// thread and thus in parallel to writing and compression. 
        /// </summary>
        PingPong = 0x01,

        /// <summary>
        /// In this mode, the user has to call the method AddFrame 
        /// for each and every frame that should be added to the AVI file.
        /// </summary>
        FrameByFrame = 0x02,

        /// <summary>
        /// In this mode, CVB Movie acquires the images using the image object's IGrab2 interface. 
        /// This is about as fast as the PingPong acquisition (note that some grabbers do not expose 
        /// the IPingPong interface but an IGrab2 interface - in those cases use this mode!).
        /// </summary>
        Grab2 = 0x03,
    }
}
