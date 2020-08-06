using System;
using System.Collections.Generic;
using Api;
using Api.Periphery;
using Api.Settings;
using Api.Camera;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace Video.Dalsa
{
    public class GenieNanoCamera : BaseCamera<MatFrame>, IPeripheral
    {
        // constants
        private const int XMin = 64;
        private const int YMin = 150;
        private const int Width = 1168;
        private const int Height = 646;

        // Members
        private readonly IWriteable<GenieNanoSettings> _options;
        private readonly ILogger _logger;
        private readonly string _name;
        private IntPtr _cameraPtr = IntPtr.Zero;

        public GenieNanoCamera(
            ILogger<GenieNanoCamera> logger,
            IWriteable<GenieNanoSettings> options)
        {
            _logger = logger;
            _options = options;
            _name = options?.Value.CameraName;

            CreateCamera();
            ApplyOptions();

            _options?.RegisterChangeListener(ApplyOptions);
        }

        ~GenieNanoCamera()
        {
            if (!_cameraPtr.Equals(IntPtr.Zero))
            {
                GenieNanoDll.DestroyCamera(_cameraPtr);
            }
        }

        public override PeripheralState PeripheralState { get; set; }

        protected override void StartAcquisition()
        {
            if (!GenieNanoDll.StartAcquisition(_cameraPtr))
            {
                throw new KickermatException($"Dalsa Camera {_name}: Failed to start acquisition");
            }
        }

        protected override void StopAcquisition()
        {
            if (!GenieNanoDll.StopAcquisition(_cameraPtr))
            {
                throw new KickermatException($"Dalsa Camera {_name}: Failed to stop acquisition");
            }
        }

        private void CreateCamera()
        {
            var roi = new RegionOfInterest(XMin, YMin, Width, Height);
            _cameraPtr = GenieNanoDll.CreateCamera(_name, roi, FrameArrived,
                CameraConnected, CameraDisconnected);
        }

        private void ApplyOptions()
        {
            if (PeripheralState != PeripheralState.Ready)
            {
                return;
            }

            if (!GenieNanoDll.SetFeatureValue(_cameraPtr, "autoBrightnessTarget",
                _options.Value.Brightness))
            {
                throw new KickermatException("Could not apply options to camera");
            }
        }

        private void CameraConnected(string name)
        {
            _logger.LogInformation($"Camera {name} connected");
            PeripheralState = PeripheralState.Ready;
        }

        private void CameraDisconnected(string name)
        {
            _logger.LogInformation($"Camera {name} disconnected");
            PeripheralState = PeripheralState.NotConnected;
        }

        private void FrameArrived(int bufferIndex, IntPtr frameAddress)
        {
            // Debayering
            var bayerMat = new Mat(Height, Width, MatType.CV_8U, frameAddress);
            var bgrMat = bayerMat.CvtColor(ColorConversionCodes.BayerBG2BGR);
            bayerMat.Dispose();
            GenieNanoDll.ReleaseBuffer(_cameraPtr, bufferIndex);

            Push(new MatFrame(bgrMat));
        }
    }
}
