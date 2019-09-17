using System;
using System.Collections.Generic;
using System.Text;
using Configuration;
using ImageProcessing.Calibration;
using ImageProcessing.Preprocessing;
using ImageProcessingNew.BarSearch;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.BarSearch
{
    public class BarSearch : BaseVideoProcessor, IBarSearch, IConfigurable<BarSearchSettings>
    {
        public IWritableOptions<BarSearchSettings> Options { get; }

        public BarSearch(IVideoSource camera, ILogger<BarSearch> logger,
            IWritableOptions<BarSearchSettings> options) : base(camera, logger)
        {
            Options = options;
        }

        public void ApplyOptions()
        {
            // Live application
        }

        protected override IFrame ProcessFrame(IFrame frame)
        {
            var img = frame.Mat;

            // Mask white lines (includes bars)
            var lower = new Scalar(0, 0, 220);
            var upper = new Scalar(255, 120, 255);
            var threshImg = img.CvtColor(ColorConversionCodes.BGR2HSV)
                .InRange(lower, upper)
                .GaussianBlur(new Size(Options.Value.BlurSize, Options.Value.BlurSize), 0)
                .Dilate(0, null, (int)Options.Value.DilationIterations)
                .Erode(0);

            var threshImgHorizontal = threshImg.Clone();
            var verticalLines = threshImg.HoughLinesP(200.0, Math.PI, 30, 100.0, 0.0);
            Console.WriteLine("#verticalLines: {0}", verticalLines.Length);
            foreach (var point in verticalLines)
            {
                img.Line(point.P1, point.P2, new Scalar(255, 0, 255));
            }
            /*
            var horizontalLines = threshImgHorizontal.HoughLinesP(200.0, 0.5*Math.PI, 30, 100.0, 0.0);
            Console.WriteLine("#horizontalLines: {0}", horizontalLines.Length);
            foreach(var point in horizontalLines)
            {
                img.Line(point.P1, point.P2, new Scalar(0, 255, 0));
            }
            */

            return new Frame(threshImg);
        }
    }
}
