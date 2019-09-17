using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public class Frame : IFrame
    {
        private Mat _mat;

        public Mat Mat {
            get
            {
                if (_mat.IsDisposed)
                    throw new VideoSourceException("Frame has already been released");
                return _mat;
            }
            private set => _mat = value;
        }

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
