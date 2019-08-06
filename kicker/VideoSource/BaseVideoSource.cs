using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    public abstract class BaseVideoSource : IVideoSource
    {
        private readonly object _objectLock = new object();
        protected EventHandler<FrameArrivedArgs> FrameArrived { get; set; }
        protected EventHandler<CameraEventArgs> CameraConnected { get; set; }
        protected EventHandler<CameraEventArgs> CameraDisconnected { get; set; }

        protected virtual void StartAcquisition() { }
        protected virtual void StopAcquisition() { }

        public void StartAcquisition(IVideoConsumer consumer)
        {
            lock (_objectLock)
            {
                FrameArrived += consumer.OnFrameArrived;
                CameraConnected += consumer.OnCameraConnected;
                CameraDisconnected += consumer.OnCameraDisconnected;

                StartAcquisition();
            }
        }

        public void StopAcquisition(IVideoConsumer consumer)
        {
            lock (_objectLock)
            {
                FrameArrived -= consumer.OnFrameArrived;
                CameraConnected -= consumer.OnCameraConnected;
                CameraDisconnected -= consumer.OnCameraDisconnected;

                StopAcquisition();
            }

        }
    }
}
