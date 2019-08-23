using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VideoSource
{
    public abstract class BaseVideoProcessor : BaseVideoSource, IVideoProcessor
    {
        public BaseVideoProcessor(ILogger<IVideoSource> logger) : base(logger)
        {
        }

        protected abstract IFrame ProcessFrame(IFrame frame);

        public void OnCameraConnected(object sender, CameraEventArgs args)
        {
            HandleConnect(args);
        }

        public void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            HandleDisconnect(args);
        }

        public void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            args.Frame = ProcessFrame(args.Frame);
            HandleFrameArrived(args);
        }
    }
}
