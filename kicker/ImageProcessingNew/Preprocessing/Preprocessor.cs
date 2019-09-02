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
            var distCoeffs = MatOfDouble.FromArray(
                _calibrationOptions.Value.DistortionCoefficients);
            var cameraMatrix = MatOfDouble.FromArray(
                ToMultiDimArray(_calibrationOptions.Value.CameraMatrix));
            
            var undistortedFrame = new Mat();
            Cv2.Undistort(frame, undistortedFrame, cameraMatrix, distCoeffs);

            return undistortedFrame;
        }

        private double[,] ToMultiDimArray(double[][] matrix)
        {
            double[,] multi = new double[matrix.Length, matrix[0].Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (var k = 0; k < matrix[0].Length; k++) {
                    multi[i, k] = matrix[i][k];
                }
            }

            return multi;
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
