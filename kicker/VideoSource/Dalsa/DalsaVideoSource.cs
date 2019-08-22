﻿using Configuration;
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
        // Parameter constants
        private const double GAIN_DEFAULT = 1.0;
        private const double GAIN_MIN = 1.0;
        private const double GAIN_MAX = 8.0;
        private const double EXPOSURE_TIME_DEFAULT = 15000.0;
        private const double EXPOSURE_TIME_MIN = 1.0;
        private const double EXPOSURE_TIME_MAX = 160000.0;

        private const string CAMERA_NAME = "Nano-C1280_1";
        private static DalsaVideoSource _dalsaVideoSource;

        [NumberParameter("Gain", "The analog gain (brightness)",
            GAIN_DEFAULT, GAIN_MIN, GAIN_MAX, 0.1)]
        public double Gain {
            get
            {
                unsafe
                {
                    double gain = 0.0;
                    if (DalsaApi.get_feat_value("Gain", &gain))
                    {
                        return gain;
                    }
                }

                throw new KickerParameterException("Failed to retrieve Gain");
            }
            set
            {
                if (value >= GAIN_MIN && value <= GAIN_MAX)
                {
                    if (!DalsaApi.set_feat_value("Gain", value))
                    {
                        throw new KickerParameterException("Failed to set Gain to " + value);
                    }
                }
            }
        }

        private double _exposureTime;
        [NumberParameter("Exposure Time", "The exposure time in microseconds",
            EXPOSURE_TIME_DEFAULT, EXPOSURE_TIME_MIN, EXPOSURE_TIME_MAX, 10)]
        public double ExposureTime
        {
            get => _exposureTime;
            set => _exposureTime = value;
        }

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
