using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VideoSource.Dalsa;

namespace VideoSource.Dalsa
{
    public class DalsaVideoSource : BaseVideoSource, IConfigurable<DalsaSettings>
    {
        // Parameter constants
        private const double GAIN_DEFAULT = 1.0;
        private const double GAIN_MIN = 1.0;
        private const double GAIN_MAX = 8.0;
        private const double EXPOSURE_TIME_DEFAULT = 15000.0;
        private const double EXPOSURE_TIME_MIN = 1.0;
        private const double EXPOSURE_TIME_MAX = 33246.0;

        private const string CAMERA_NAME = "Nano-C1280_1";
        private static DalsaVideoSource _dalsaVideoSource;
        private readonly object _mutex = new object();

        public IWritableOptions<DalsaSettings> Options { get; set; }

        [NumberParameter("Gain", "The analog gain (brightness)",
            GAIN_DEFAULT, GAIN_MIN, GAIN_MAX, 0.1)]
        public double Gain {
            get
            {
                return getDoubleParameter("Gain");
            }
            set
            {
                setDoubleParameter("Gain", value, GAIN_MIN, GAIN_MAX);
            }
        }

        [NumberParameter("Exposure Time", "The exposure time in microseconds",
            EXPOSURE_TIME_DEFAULT, EXPOSURE_TIME_MIN, EXPOSURE_TIME_MAX, 10)]
        public double ExposureTime
        {
            get
            {
                return getDoubleParameter("ExposureTime");
            }
            set
            {
                setDoubleParameter("ExposureTime", value, EXPOSURE_TIME_MIN, EXPOSURE_TIME_MAX);
            }
        }

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger, IWritableOptions<DalsaSettings>
            options) : base(logger)
        {
            _dalsaVideoSource = this;
            Options = options;
        }

        protected override void StartAcquisition()
        {
            lock (_mutex)
            {
                DalsaApi.startup(OnFrameArrived, ServerConnected, ServerDisconnected);
                if (!DalsaApi.start_acquisition(CAMERA_NAME))
                {
                    DalsaApi.shutdown();
                    // TODO: Enrich error with more information (SapManager::GetLastStatus())
                    throw new VideoSourceException("Failed to start the acquisition");
                }
            }
        }

        protected override void StopAcquisition()
        {
            lock (_mutex)
            {
                DalsaApi.stop_acquisition();
                DalsaApi.shutdown();
            }
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

        private void setDoubleParameter(string parameterName, double value, double min, double max)
        {
            lock (_mutex)
            {
                if (value < min || value > max)
                {
                    var msg = string.Format("Cannot set parameter {0} to {1}: " +
                        "Out of bounds (Min: {2}, Max: {3})",
                        parameterName, value, EXPOSURE_TIME_MIN, EXPOSURE_TIME_MAX);
                    throw new KickerParameterException(msg);
                }

                if (!DalsaApi.set_feat_value(CAMERA_NAME, parameterName, value))
                {
                    var msg = string.Format("Failed to set {0} to {1}", parameterName, value);
                    throw new KickerParameterException(msg);
                }
            }
        }

        private double getDoubleParameter(string parameterName)
        {
            lock (_mutex)
            {
                unsafe
                {
                    double param = 0.0;
                    if (DalsaApi.get_feat_value(CAMERA_NAME, parameterName, &param))
                    {
                        return param;
                    }
                }

                var msg = string.Format("Failed to retrieve value of {0} parameter", parameterName);
                throw new KickerParameterException(msg);
            }
        }
    }
}
