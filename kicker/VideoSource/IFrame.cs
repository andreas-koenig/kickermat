using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    interface IFrame
    {
        Mat Mat { get; set; }

        void Release();
    }
}
