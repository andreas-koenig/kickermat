using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    public interface IVideoSource
    {
        event EventHandler<FrameArrivedArgs> FrameArrived;
    }

    public class FrameArrivedArgs
    {
        public IFrame Frame { get; }

        public FrameArrivedArgs(IFrame frame)
        {
            Frame = frame;
        }
    }
}
