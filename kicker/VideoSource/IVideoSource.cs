using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    public interface IVideoSource
    {
        event EventHandler<FrameArrivedArgs> FrameArrived;
        event EventHandler<CameraEventArgs> CameraDisconnected;
        event EventHandler<CameraEventArgs> CameraConnected;
    }

    public class FrameArrivedArgs
    {
        public IFrame Frame { get; }

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
