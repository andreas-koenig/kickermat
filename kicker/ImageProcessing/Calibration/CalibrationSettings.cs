using System;
using System.Collections.Generic;
using System.Text;
using Configuration.Parameter;
using OpenCvSharp;

namespace ImageProcessing.Calibration
{
    [KickermatSettings(typeof(CameraCalibration), "Camera", "Calibration")]
    public class CalibrationSettings
    {
        public double[][] CameraMatrix { get; set; }

        public double[] DistortionCoefficients { get; set; }

        public Mat GetCameraMatrixAsMat()
        {
            return MatOfDouble.FromArray(ToMultiDimArray(CameraMatrix));
        }

        public Mat GetDistCoeffsAsMat()
        {
            return MatOfDouble.FromArray(DistortionCoefficients);
        }

        private static double[,] ToMultiDimArray(double[][] matrix)
        {
            double[,] multi = new double[matrix.Length, matrix[0].Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (var k = 0; k < matrix[0].Length; k++)
                {
                    multi[i, k] = matrix[i][k];
                }
            }

            return multi;
        }
    }
}
