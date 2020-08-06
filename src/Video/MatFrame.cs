using System;
using System.Collections.Generic;
using System.Text;
using Api.Camera;
using OpenCvSharp;

namespace Video
{
    public class MatFrame : IFrame
    {
        public MatFrame(Mat frame)
        {
            Mat = frame;
        }

        public Mat Mat { get; protected set; }

        public byte[] ToBytes()
        {
            return Mat.ToBytes();
        }
    }
}
