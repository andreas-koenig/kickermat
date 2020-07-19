using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public class Frame : IFrame
    {
        private Mat _mat;
        private int _numSubscribers = 1;

        public Frame(Mat mat)
        {
            Mat = mat;
        }

        internal Frame(Mat mat, int numSubscribers)
            : this(mat)
        {
            _numSubscribers = numSubscribers;
        }

        public Mat Mat
        {
            get
            {
                if (_mat.IsDisposed)
                {
                    throw new VideoSourceException("Frame has already been released");
                }

                return _mat;
            }
            private set => _mat = value;
        }

        public void Release()
        {
            _numSubscribers--;
            if (_numSubscribers == 0)
            {
                Mat.Release();
            }
        }
    }
}
