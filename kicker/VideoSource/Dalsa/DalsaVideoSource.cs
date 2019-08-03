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
        private EventHandler<FrameArrivedArgs> frameArrived;
        public event EventHandler<FrameArrivedArgs> FrameArrived
        {
            add
            {
                lock(objectLock)
                {
                    frameArrived += value;
                    if (frameArrived.GetInvocationList().Length == 1)
                    {
                        StartAcquisition();
                    }
                }
            }
            remove
            {
                lock(objectLock)
                {
                    if (frameArrived.GetInvocationList().Length == 1)
                    {
                        StopAcquisition();
                    }
                    frameArrived -= value;
                }
            }
        }

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger)
        {
            _dalsaVideoSource = this;
            _logger = logger;
        }

        internal void StartAcquisition()
        {
            DalsaApi.start_acquisition(CreateMat);
            _logger.LogInformation("Acquisition started");
        }

        internal void StopAcquisition()
        {
            DalsaApi.stop_acquisition();
            _logger.LogInformation("Acquisition stopped");
        }

        private static void CreateMat(int index, IntPtr address)
        {
            // Debayering of monochrome image
            Mat colorMat = new Mat();
            var monoMat = new Mat(1024, 1280, MatType.CV_8U, address);
            Cv2.CvtColor(monoMat, colorMat, ColorConversionCodes.BayerBG2BGR);

            var frame = new DalsaFrame(colorMat);
            DalsaApi.release_buffer(index);

            _dalsaVideoSource?.OnFrameArrived(frame);
        }

        protected virtual void OnFrameArrived(IFrame frame)
        {
            frameArrived?.Invoke(this, new FrameArrivedArgs(frame));
        }
    }
}
