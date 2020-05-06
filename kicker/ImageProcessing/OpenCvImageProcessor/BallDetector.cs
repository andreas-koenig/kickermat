using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.OpenCvImageProcessor
{
    internal class BallDetector
    {
        private const string Threshold = "ballThreshold";

        private IWritableOptions<ImageProcessorSettings> _options;

        public BallDetector(IWritableOptions<ImageProcessorSettings> options)
        {
            _options = options;
        }

        public IEnumerable<Channel> GetChannels()
        {
            return new Channel[]
            {
                new Channel(Threshold, "Ball Threshold", "The mask for the ball detection"),
            };
        }

        internal (Mat, Rect) DetectBall(Mat img, string channel)
        {
            var ballColor = _options.Value.BallColor;
            var lower = ImageProcessor.HsvToScalar(ballColor.Lower);
            var upper = ImageProcessor.HsvToScalar(ballColor.Upper);
            var threshImg = img
                .CvtColor(ColorConversionCodes.BGR2HSV)
                .InRange(lower, upper);

            threshImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);

            try
            {
                var ballRect = ImageProcessor.GetBoundingRects(contours)
                    .OrderBy(rect => rect.Width * rect.Height)
                    .Last();

                switch (channel)
                {
                    case Threshold:
                        threshImg.Rectangle(ballRect, new Scalar(255, 0, 0), 2);
                        return (threshImg, ballRect);

                    default:
                        return (null, ballRect);
                }
            }
            catch (Exception ex)
            {
                return (threshImg, new Rect(0, 0, 10, 10));
            }
        }
    }
}
