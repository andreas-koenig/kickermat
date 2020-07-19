using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public interface IVideoSource
    {
        Channel Channel { get; set; }

        /// <summary>
        /// Start the acquisition of frames from the VideoSource.
        /// </summary>
        /// <param name="consumer">A consumer implementing the callbacks defined in the
        /// IVideoConsumer interface.</param>
        /// <exception cref="VideoSourceException">Thrown when the acquisition cannot be started.
        /// </exception>
        void StartAcquisition(IVideoConsumer consumer);

        /// <summary>
        /// Stop the acquisition of frames.
        /// </summary>
        /// <param name="consumer">The consumer using the frames</param>
        void StopAcquisition(IVideoConsumer consumer);

        /// <summary>
        /// Get the channels of the VideoSource.
        /// </summary>
        /// <returns>the channels.</returns>
        IEnumerable<Channel> GetChannels();
    }
}
