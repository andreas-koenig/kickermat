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

        public object Clone()
        {
            return new MatFrame(Mat.Clone());
        }

        public byte[] ToBytes()
        {
            return Mat.ToBytes();
        }

        public byte[] ToJpg()
        {
            return Mat.ToJpg();
        }

        public void Dispose()
        {
            if (!Mat.IsDisposed)
            {
                Mat.Dispose();
            }
        }
    }
}
