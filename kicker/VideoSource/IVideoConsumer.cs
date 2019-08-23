using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public interface IVideoConsumer
    {
        /// <summary>
        /// This event handler is called each time the video source generates a new frame.
        /// </summary>
        /// <param name="sender">The IVideoSource generating the frames</param>
        /// <param name="args"></param>
        void OnFrameArrived(object sender, FrameArrivedArgs args);

        /// <summary>
        /// This event handler is called in case the camera physically disconnected. There is no
        /// need to stop the acquisition as it automatically continues after a reconnect. The
        /// intended use of this callback is to inform the user about the resulting freeze of the
        /// video stream.
        /// </summary>
        /// <param name="sender">The IVideoSource generating the frames</param>
        /// <param name="args"></param>
        void OnCameraDisconnected(object sender, CameraEventArgs args);

        /// <summary>
        /// This event handler is called when the camera is reconnected after a disconnect. The
        /// frame acquisition automatically continues.
        /// </summary>
        /// <param name="sender">The IVideoSource generating the frames</param>
        /// <param name="args"></param>
        void OnCameraConnected(object sender, CameraEventArgs args);
    }

    public class FrameArrivedArgs
    {
        public IFrame Frame { get; set; }

        public FrameArrivedArgs(Mat mat)
        {
            Frame = new Frame(mat);
        }

        public FrameArrivedArgs(IFrame frame)
        {
            Frame = frame;
        }
    }

    public class CameraEventArgs
    {
        public string CameraName { get; }

        public CameraEventArgs(string cameraName)
        {
            CameraName = cameraName;
        }
    }
}
