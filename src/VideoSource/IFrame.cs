using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace VideoSource
{
    public interface IFrame
    {
        Mat Mat { get; }

        void Release();
    }
}
