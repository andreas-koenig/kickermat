using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public interface IVideoSource
    {
        /// <summary>
        /// Start the acquisition of frames from the VideoSource. If
        /// </summary>
        /// <param name="consumer">A consumer implementing the callbacks defined in the
        /// IVideoConsumer interface</param>
        void StartAcquisition(IVideoConsumer consumer);

        /// <summary>
        /// Stop the acquisition of frames.
        /// </summary>
        /// <param name="consumer">The consumer using the frames</param>
        void StopAcquisition(IVideoConsumer consumer);
    }
}
