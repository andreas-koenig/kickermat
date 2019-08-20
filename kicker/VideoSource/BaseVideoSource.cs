﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VideoSource
{
    public abstract class BaseVideoSource : IVideoSource
    {
        protected ILogger<IVideoSource> Logger { get; private set; }
        private readonly object _objectLock = new object();
        private int _consumerCount = 0;
        private bool _acquisitionRunning = false;

        protected EventHandler<FrameArrivedArgs> FrameArrived { get; set; }
        protected EventHandler<CameraEventArgs> CameraConnected { get; set; }
        protected EventHandler<CameraEventArgs> CameraDisconnected { get; set; }

        public BaseVideoSource(ILogger<IVideoSource> logger)
        {
            Logger = logger;
        }

        protected virtual void StartAcquisition() { }
        protected virtual void StopAcquisition() { }

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
                _consumerCount = newConsumerCount == null ? 0 : (int)newConsumerCount;

                if (_consumerCount > 1)
                {
                    Logger.LogInformation("Added consumer ({} in total)", _consumerCount);
                    return;
                }
            }
            try
            {
                StartAcquisition();
                _acquisitionRunning = true;

                Logger.LogInformation("Acquisition started ({} consumers)", _consumerCount);
            }
            catch (VideoSourceException ex)
            {
                lock (_objectLock)
                {
                    FrameArrived -= consumer.OnFrameArrived;
                    CameraConnected -= consumer.OnCameraConnected;
                    CameraDisconnected -= consumer.OnCameraDisconnected;
                    _consumerCount -= 1;

                    Logger.LogError(ex, "Failed to start camera acquisition");
                }

                throw ex;
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
                if (oldConsumerCount == null && newConsumerCount == null ||
                    !(oldConsumerCount != null && newConsumerCount == null) ||
                    oldConsumerCount <= newConsumerCount)
                {
                    return;
                }

                _consumerCount -= 1;

                if (_acquisitionRunning && _consumerCount == 0)
                {
                    StopAcquisition();
                    _acquisitionRunning = false;
                    Logger.LogInformation("Acquisition stopped");
                    return;
                }

                Logger.LogInformation("Consumer unsubscribed ({} remaining)", _consumerCount);
            }
        }

        protected void HandleFrameArrived(FrameArrivedArgs args)
        {
            FrameArrived?.Invoke(this, args);
        }

        protected void HandleConnect(CameraEventArgs args)
        {
            Logger.LogInformation("Camera ({}) connected. Notifying {} consumers",
                args.CameraName, _consumerCount);
            CameraConnected?.Invoke(this, args);
        }

        protected void HandleDisconnect(CameraEventArgs args)
        {
            Logger.LogInformation("Camera ({}) disconnected. Notifying {} consumers",
                args.CameraName, _consumerCount);
            CameraDisconnected?.Invoke(this, args);
        }
    }
}