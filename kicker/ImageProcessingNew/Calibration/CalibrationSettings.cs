using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace ImageProcessing.Calibration
{
    public class CalibrationSettings
    {
        public double[][] CameraMatrix { get; set; }
        public double[] DistortionCoefficients { get; set; }

        public CalibrationSettings() { }
    }
}
