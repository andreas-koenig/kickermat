using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource.Dalsa
{
    class DalsaFrame : IFrame
    {
        internal DalsaFrame(Mat mat)
        {
            Mat = mat;
        }

        public Mat Mat { get; set; }

        public void Release()
        {
            // Not needed as image buffer is released right away.
        }
    }
}
