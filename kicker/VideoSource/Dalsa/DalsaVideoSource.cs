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
        private const string CAMERA_NAME = "Nano-C1280_1";
        private static DalsaVideoSource _dalsaVideoSource;

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger) : base(logger)
        {
            _dalsaVideoSource = this;
        }

        protected override void StartAcquisition()
        {
            DalsaApi.startup(OnFrameArrived, ServerConnected, ServerDisconnected);
            if (!DalsaApi.start_acquisition(CAMERA_NAME))
            {
                DalsaApi.shutdown();
                // TODO: Enrich error with more information (SapManager::GetLastStatus())
                throw new VideoSourceException("Failed to start the acquisition");
            }
        }

        protected override void StopAcquisition()
        {
            DalsaApi.stop_acquisition();
            DalsaApi.shutdown();
        }

        private static void ServerConnected(string serverName)
        {
            _dalsaVideoSource?.HandleConnect(new CameraEventArgs(serverName));
        }

        private static void ServerDisconnected(string serverName)
        {
            _dalsaVideoSource?.HandleDisconnect(new CameraEventArgs(serverName));
        }

        private static void OnFrameArrived(int index, IntPtr address)
        {
            // Debayering of monochrome image
            Mat colorMat = new Mat();
            var monoMat = new Mat(1024, 1280, MatType.CV_8U, address);
            Cv2.CvtColor(monoMat, colorMat, ColorConversionCodes.BayerBG2BGR);

            var frame = new DalsaFrame(colorMat);
            DalsaApi.release_buffer(index);

            _dalsaVideoSource.HandleFrameArrived(new FrameArrivedArgs(frame));
        }
    }
}
