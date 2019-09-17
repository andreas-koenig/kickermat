using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.Preprocessing
{
    public interface IPreprocessor : IVideoSource, IVideoConsumer
    {
        Mat UndistortFrame(Mat frame);
    }
}
