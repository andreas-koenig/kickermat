﻿using System;
using System.Collections.Generic;
using System.Text;
using Api.Settings.Parameter;
using OpenCvSharp;

namespace Video.Calibration
{
    public class CalibrationSettings
    {
        public double[][] CameraMatrix { get; set; }

        public double[] DistortionCoefficients { get; set; }

        public Mat GetCameraMatrixAsMat()
        {
            return Mat.FromArray(ToMultiDimArray(CameraMatrix));
        }

        public Mat GetDistCoeffsAsMat()
        {
            return Mat.FromArray(DistortionCoefficients);
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
