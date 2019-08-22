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
            HandleFrameArrived(args);
        }

        public void OnCameraConnected(object sender, CameraEventArgs args)
        {
            HandleConnect(args);
        }

        public void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            HandleDisconnect(args);
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
