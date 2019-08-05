using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoSource.Dalsa
{
    public class DalsaVideoSource : IVideoSource
    {
        private static DalsaVideoSource _dalsaVideoSource;
        private ILogger _logger;

        private object objectLock = new object();
        private EventHandler<FrameArrivedArgs> _frameArrived;
        public event EventHandler<FrameArrivedArgs> FrameArrived
        {
            add
            {
                lock (objectLock)
                {
                    _frameArrived += value;
                    if (_frameArrived.GetInvocationList().Length == 1)
                    {
                        StartAcquisition();
                    }
                }
            }
            remove
            {
                lock (objectLock)
                {
                    if (_frameArrived.GetInvocationList().Length == 1)
                    {
                        StopAcquisition();
                    }
                    _frameArrived -= value;
                }
            }
        }

        private EventHandler<CameraEventArgs> _cameraDisconnected;
        public event EventHandler<CameraEventArgs> CameraDisconnected {
            add
            {
                lock (objectLock)
                {
                    _cameraDisconnected += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    _cameraDisconnected -= value;
                }
            }
        }

        private EventHandler<CameraEventArgs> _cameraConnected;
        public event EventHandler<CameraEventArgs> CameraConnected
        {
            add
            {
                lock (objectLock)
                {
                    _cameraConnected += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    _cameraConnected -= value;
                }
            }
        }

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger)
        {
            _dalsaVideoSource = this;
            _logger = logger;
            DalsaApi.startup(OnFrameArrived, ServerConnected, ServerDisconnected);
        }

        private static void ServerConnected(string serverName)
        {
            _dalsaVideoSource._logger.LogInformation(serverName + " connected");
            _dalsaVideoSource?._cameraConnected
                ?.Invoke(_dalsaVideoSource, new CameraEventArgs(serverName));
        }

        private static void ServerDisconnected(string serverName)
        {
            _dalsaVideoSource._logger.LogInformation(serverName + " disconnected");
            _dalsaVideoSource?._cameraDisconnected
                ?.Invoke(_dalsaVideoSource, new CameraEventArgs(serverName));
        }

        private void StartAcquisition()
        {
            DalsaApi.start_acquisition("test");
            _logger.LogInformation("Acquisition started");
        }


        private void StopAcquisition()
        {
            DalsaApi.stop_acquisition();
            _logger.LogInformation("Acquisition stopped");
        }

        private static void OnFrameArrived(int index, IntPtr address)
        {
            // Debayering of monochrome image
            Mat colorMat = new Mat();
            var monoMat = new Mat(1024, 1280, MatType.CV_8U, address);
            Cv2.CvtColor(monoMat, colorMat, ColorConversionCodes.BayerBG2BGR);

            var frame = new DalsaFrame(colorMat);
            DalsaApi.release_buffer(index);

            _dalsaVideoSource?._frameArrived
                ?.Invoke(_dalsaVideoSource, new FrameArrivedArgs(frame));
        }
    }
}
