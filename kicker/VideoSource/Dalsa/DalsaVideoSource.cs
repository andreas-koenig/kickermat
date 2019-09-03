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

        public IWritableOptions<DalsaSettings> Options { get; }

        public DalsaVideoSource(ILogger<DalsaVideoSource> logger, IWritableOptions<DalsaSettings>
            options) : base(logger)
        {
            _dalsaVideoSource = this;
            Options = options;
        }

        public void ApplyOptions()
        {
            SetBrightness((int)Options.Value.Brightness);
            //SetExposureTime(Options.Value.ExposureTime);
        }

        protected override void StartAcquisition()
        {
            lock (_mutex)
            {
                DalsaApi.startup(OnFrameArrived, ServerConnected, ServerDisconnected);
                if (!DalsaApi.start_acquisition(Options.Value.CameraName))
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
            var img = new Mat(1024, 1280, MatType.CV_8UC3, address);
            var frame = new DalsaFrame(img);
            DalsaApi.release_buffer(index);
            _dalsaVideoSource.HandleFrameArrived(new FrameArrivedArgs(frame));
        }

        private void SetExposureTime(double value)
        {
            lock (_mutex)
            {
                if (value < DalsaSettings.EXPOSURE_TIME_MIN ||
                    value > DalsaSettings.EXPOSURE_TIME_MAX)
                {
                    var msg = string.Format("Cannot set parameter exposure time to {0}: " +
                        "Out of bounds (Min: {1}, Max: {2})", value,
                        DalsaSettings.EXPOSURE_TIME_MIN, DalsaSettings.EXPOSURE_TIME_MAX);
                    throw new KickerParameterException(msg);
                }

                unsafe
                {
                    if (!DalsaApi.set_exposure_time(CAMERA_NAME, value))
                    {
                        var msg = string.Format("Failed to set exposure time to {0}", value);
                        throw new KickerParameterException(msg);
                    }
                }
            }
        }

        private void SetBrightness(int value)
        {
            lock (_mutex)
            {
                if (value < DalsaSettings.BRIGHTNESS_MIN ||
                    value > DalsaSettings.BRIGHTNESS_MAX)
                {
                    var msg = string.Format("Cannot set parameter brightness to {0}: " +
                        "Out of bounds (Min: {1}, Max: {2})", value,
                        DalsaSettings.BRIGHTNESS_MIN, DalsaSettings.BRIGHTNESS_MAX);
                    throw new KickerParameterException(msg);
                }

                unsafe
                {
                    if (!DalsaApi.set_brightness(CAMERA_NAME, value))
                    {
                        var msg = string.Format("Failed to set brightness to {0}", value);
                        throw new KickerParameterException(msg);
                    }
                }
            }
        }
    }
}
