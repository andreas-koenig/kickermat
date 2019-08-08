using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoSource.Dalsa
{
    public class DalsaVideoSource : BaseVideoSource
    {
        private static DalsaVideoSource _dalsaVideoSource;
        private readonly ILogger _logger;

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger)
        {
            _dalsaVideoSource = this;
            _logger = logger;
            DalsaApi.startup(OnFrameArrived, ServerConnected, ServerDisconnected);
        }

        protected override void StartAcquisition()
        {
            if (FrameArrived.GetInvocationList().Length == 1)
            {
                DalsaApi.start_acquisition("test");
                _logger.LogInformation("Acquisition started");
            }
        }

        protected override void StopAcquisition()
        {
            if (FrameArrived == null)
            {
                DalsaApi.stop_acquisition();
                _logger.LogInformation("Acquisition stopped");
            }
        }

        private static void ServerConnected(string serverName)
        {
            _dalsaVideoSource._logger.LogInformation(serverName + " connected");
            _dalsaVideoSource?.CameraConnected
                ?.Invoke(_dalsaVideoSource, new CameraEventArgs(serverName));
        }

        private static void ServerDisconnected(string serverName)
        {
            _dalsaVideoSource._logger.LogInformation(serverName + " disconnected");
            _dalsaVideoSource?.CameraDisconnected
                ?.Invoke(_dalsaVideoSource, new CameraEventArgs(serverName));
        }

        private static void OnFrameArrived(int index, IntPtr address)
        {
            // Debayering of monochrome image
            Mat colorMat = new Mat();
            var monoMat = new Mat(1024, 1280, MatType.CV_8U, address);
            Cv2.CvtColor(monoMat, colorMat, ColorConversionCodes.BayerBG2BGR);

            var frame = new DalsaFrame(colorMat);
            DalsaApi.release_buffer(index);

            _dalsaVideoSource?.FrameArrived
                ?.Invoke(_dalsaVideoSource, new FrameArrivedArgs(frame));
        }
    }
}
