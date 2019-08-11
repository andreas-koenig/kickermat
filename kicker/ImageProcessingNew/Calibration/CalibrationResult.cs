using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using OpenCvSharp;

namespace ImageProcessing.Calibration
{
    public class CalibrationResult
    {
        public Mat CameraMatrix { get; }
        public Mat DistortionCoefficients { get; }
        public double ReprojectionError { get; }

        public CalibrationResult(Mat cameraMatrix, Mat distCoeffs, double rms)
        {
            CameraMatrix = cameraMatrix;
            DistortionCoefficients = distCoeffs;
            ReprojectionError = rms;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Camera Matrix:\n");
            builder.Append(Cv2.Format(CameraMatrix, FormatType.Python));

            builder.Append("\nDistortion Coefficients:\n");
            builder.Append(Cv2.Format(DistortionCoefficients, FormatType.Python));

            return builder.ToString();
        }
    }
}
