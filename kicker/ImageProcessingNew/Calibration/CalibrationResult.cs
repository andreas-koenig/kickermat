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
            AddMatToString(CameraMatrix, builder);

            builder.Append("\nDistortion Coefficients:\n");
            AddMatToString(DistortionCoefficients, builder);

            return builder.ToString();
        }

        private void AddMatToString(Mat mat, StringBuilder builder)
        {
            var numberFormat = NumberFormatInfo.InvariantInfo;

            builder.Append("[");
            for (int i = 0; i < mat.Width; i++)
            {
                builder.Append("[");
                for (int j = 0; j < mat.Height; j++)
                {
                    builder.Append(mat.At<double>(i, j).ToString(numberFormat));
                    if (j < (mat.Height - 1))
                        builder.Append(", ");
                }
                builder.Append("]");
                if (i < (mat.Width - 1))
                    builder.Append("\n");
            }
            builder.Append("]\n");
        }
    }
}
