using System;
using System.Collections.Generic;
using System.Text;
using Configuration;
using ImageProcessing.Calibration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.Preprocessing
{
    public class Preprocessor : BaseVideoProcessor, IPreprocessor
    {
        private readonly ILogger<Preprocessor> _logger;

        // Input
        private IVideoSource _camera;

        // Distortion Correction
        private IWritableOptions<CalibrationSettings> _calibrationOptions;

        public Preprocessor(ILogger<Preprocessor> logger, IVideoSource camera,
            IWritableOptions<CalibrationSettings> calibrationOptions) : base(logger)
        {
            _logger = logger;
            _calibrationOptions = calibrationOptions;
            _camera = camera;
        }

        protected override IFrame ProcessFrame(IFrame frame)
        {
            frame.Mat = UndistortFrame(frame.Mat);
            return frame;
        }

        public Mat UndistortFrame(Mat frame)
        {
            var distCoeffs = _calibrationOptions.Value.GetDistCoeffsAsMat();
            var cameraMatrix = _calibrationOptions.Value.GetCameraMatrixAsMat();

            return frame.Undistort(cameraMatrix, distCoeffs);
        }

        protected override void StartAcquisition()
        {
            _camera.StartAcquisition(this);
        }

        protected override void StopAcquisition()
        {
            _camera.StopAcquisition(this);
        }
    }
}
