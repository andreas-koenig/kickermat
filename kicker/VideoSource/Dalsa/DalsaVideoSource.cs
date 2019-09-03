using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System;

namespace VideoSource.Dalsa
{
    public class DalsaVideoSource : BaseVideoSource, IConfigurable<DalsaSettings>
    {
        private const string CAMERA_NAME = "Nano-C1280_1";
        private static DalsaVideoSource _dalsaVideoSource;
        private readonly object _mutex = new object();

        public IWritableOptions<DalsaSettings> Options { get; set; }

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger, IWritableOptions<DalsaSettings>
            options) : base(logger)
        {
            _dalsaVideoSource = this;
            Options = options;
        }

        public void ApplyOptions()
        {
            SetDoubleParameter("ExposureTime", Options.Value.ExposureTime,
                DalsaSettings.EXPOSURE_TIME_MIN, DalsaSettings.EXPOSURE_TIME_MAX);

            SetDoubleParameter("Gain", Options.Value.Gain, DalsaSettings.GAIN_MIN,
                DalsaSettings.GAIN_MAX);
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

                ApplyOptions();
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

        private void SetDoubleParameter(string parameterName, double value, double min, double max)
        {
            lock (_mutex)
            {
                if (value < min || value > max)
                {
                    var msg = string.Format("Cannot set parameter {0} to {1}: " +
                        "Out of bounds (Min: {2}, Max: {3})", parameterName, value,
                        DalsaSettings.EXPOSURE_TIME_MIN, DalsaSettings.EXPOSURE_TIME_MAX);
                    throw new KickerParameterException(msg);
                }

                if (!DalsaApi.set_feat_value(CAMERA_NAME, parameterName, value))
                {
                    var msg = string.Format("Failed to set {0} to {1}", parameterName, value);
                    throw new KickerParameterException(msg);
                }
            }
        }

        private double GetDoubleParameter(string parameterName)
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
