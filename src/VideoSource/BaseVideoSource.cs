using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoSource
{
    public abstract class BaseVideoSource : IVideoSource
    {
        private readonly object _objectLock = new object();
        private Channel _channel;

        public BaseVideoSource(ILogger logger)
        {
            Logger = logger;
        }

        public Channel Channel
        {
            get
            {
                lock (_objectLock)
                {
                    return _channel;
                }
            }

            set
            {
                lock (_objectLock)
                {
                    _channel = value;
                }
            }
        }

        protected bool IsAcquisitionRunning { get; set; } = false;

        protected int ConsumerCount { get; private set; } = 0;

        protected ILogger Logger { get; private set; }

        private EventHandler<FrameArrivedArgs> FrameArrived { get; set; }

        private EventHandler<CameraEventArgs> CameraConnected { get; set; }

        private EventHandler<CameraEventArgs> CameraDisconnected { get; set; }

        public void StartAcquisition(IVideoConsumer consumer)
        {
            lock (_objectLock)
            {
                // Try to unregister first to prevent multiple subcriptions by the same consumer
                FrameArrived -= consumer.OnFrameArrived;
                CameraConnected -= consumer.OnCameraConnected;
                CameraDisconnected -= consumer.OnCameraDisconnected;

                FrameArrived += consumer.OnFrameArrived;
                CameraConnected += consumer.OnCameraConnected;
                CameraDisconnected += consumer.OnCameraDisconnected;

                var newConsumerCount = FrameArrived?.GetInvocationList().Length;
                ConsumerCount = newConsumerCount == null ? 0 : (int)newConsumerCount;

                if (ConsumerCount > 1)
                {
                    Logger.LogInformation("Added consumer ({} in total)", ConsumerCount);
                    return;
                }
            }

            try
            {
                StartAcquisition();
                IsAcquisitionRunning = true;

                Logger.LogInformation("Acquisition started ({} consumers)", ConsumerCount);
            }
            catch (VideoSourceException ex)
            {
                lock (_objectLock)
                {
                    FrameArrived -= consumer.OnFrameArrived;
                    CameraConnected -= consumer.OnCameraConnected;
                    CameraDisconnected -= consumer.OnCameraDisconnected;
                    ConsumerCount -= 1;

                    var msg = "Failed to start camera acquitision";
                    Logger.LogError(ex, msg);
                    throw new VideoSourceException(msg, ex);
                }
            }
        }

        public void StopAcquisition(IVideoConsumer consumer)
        {
            lock (_objectLock)
            {
                var oldConsumerCount = FrameArrived?.GetInvocationList().Length;
                FrameArrived -= consumer.OnFrameArrived;
                CameraConnected -= consumer.OnCameraConnected;
                CameraDisconnected -= consumer.OnCameraDisconnected;
                var newConsumerCount = FrameArrived?.GetInvocationList().Length;

                // Return if consumer was not registered
                if ((oldConsumerCount == null && newConsumerCount == null) ||
                    (oldConsumerCount != null &&
                     newConsumerCount != null &&
                     oldConsumerCount <= newConsumerCount))
                {
                    return;
                }

                ConsumerCount -= 1;

                if (IsAcquisitionRunning && ConsumerCount == 0)
                {
                    StopAcquisition();
                    IsAcquisitionRunning = false;
                    Logger.LogInformation("Acquisition stopped");
                    return;
                }

                Logger.LogInformation("Consumer unsubscribed ({} remaining)", ConsumerCount);
            }
        }

        public abstract IEnumerable<Channel> GetChannels();

        protected virtual void StartAcquisition() { }

        protected virtual void StopAcquisition() { }

        protected void HandleFrameArrived(FrameArrivedArgs args)
        {
            FrameArrived?.Invoke(this, args);
        }

        protected void HandleConnect(CameraEventArgs args)
        {
            Logger.LogInformation(
                "Camera ({}) connected. Notifying {} consumers",
                args.CameraName, ConsumerCount);
            CameraConnected?.Invoke(this, args);
        }

        protected void HandleDisconnect(CameraEventArgs args)
        {
            Logger.LogInformation(
                "Camera ({}) disconnected. Notifying {} consumers",
                args.CameraName, ConsumerCount);
            CameraDisconnected?.Invoke(this, args);
        }
    }
}
