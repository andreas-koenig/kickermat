using System;
using System.Collections.Generic;
using System.Linq;
using Api.Settings;
using Api.Settings.Parameter;
using OpenCvSharp;
using Video;

namespace Webapp.Player.Classic.Vision
{
    internal class BallDetector
    {
        private const string Threshold = "ballThreshold";

        public Rect DetectBall(Mat img, ColorRange ballColor)
        {
            var lower = ClassicImageProcessor.HsvToScalar(ballColor.Lower);
            var upper = ClassicImageProcessor.HsvToScalar(ballColor.Upper);
            var threshImg = img
                .CvtColor(ColorConversionCodes.BGR2HSV)
                .InRange(lower, upper);

            threshImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);

            var ballRect = ClassicImageProcessor.GetBoundingRects(contours)
                .OrderBy(rect => rect.Width * rect.Height)
                .Last();

            img.Rectangle(ballRect, new Scalar(255, 0, 0), 2);

            return ballRect;
        }
    }
}
