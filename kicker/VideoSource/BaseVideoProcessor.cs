using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace VideoSource
{
    public abstract class BaseVideoProcessor : BaseVideoSource, IVideoProcessor
    {
        private readonly IVideoSource _videoSource;

        public BaseVideoProcessor(IVideoSource videoSource, ILogger logger)
            : base(logger)
        {
            _videoSource = videoSource;
        }

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

        protected abstract IFrame ProcessFrame(IFrame frame);

        protected override void StartAcquisition()
        {
            _videoSource.StartAcquisition(this);
        }

        protected override void StopAcquisition()
        {
            _videoSource.StopAcquisition(this);
        }
    }
}
