using System;
using System.Collections.Generic;
using System.Text;
using ImageProcessing.Calibration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.Preprocessing
{
    public class Preprocessor : BaseVideoSource, IPreprocessor
    {
        private CalibrationResult _calibrationResult;

        public Preprocessor(ILogger<Preprocessor> logger) : base(logger)
        {
            // TODO: Read calibration result from settings
        }

        public void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            args.Frame.Mat = UndistortFrame(args.Frame.Mat);
            FrameArrived?.Invoke(this, args);
        }

        public void OnCameraConnected(object sender, CameraEventArgs args)
        {
            CameraConnected?.Invoke(this, args);
        }

        public void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            CameraDisconnected?.Invoke(this, args);
        }

        private Mat UndistortFrame(Mat frame)
        {
            var undistortedFrame = new Mat();
            Cv2.Undistort(frame, undistortedFrame, _calibrationResult.CameraMatrix,
                _calibrationResult.DistortionCoefficients);

            return undistortedFrame;
        }
    }
}
