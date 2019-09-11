using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public class Frame : IFrame
    {
        public Mat Mat { get; set; }

        public Frame(Mat mat)
        {
            Mat = mat;
        }

        public void Release()
        {
            Mat.Dispose();
        }
    }
}
