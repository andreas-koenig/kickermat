using System;
using System.Collections.Generic;
using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing
{
    public class ImageProcessor : BaseVideoProcessor, IImageProcessor
    {
        private readonly IWritableOptions<ImageProcessorSettings> _options;

        public ImageProcessor(IVideoSource camera, ILogger<ImageProcessor> logger,
            IWritableOptions<ImageProcessorSettings> options)
            : base(camera, logger)
        {
            _options = options;
        }

        protected override IFrame ProcessFrame(IFrame frame)
        {
            var img = frame.Mat;

            try
            {
                // Mask white lines (includes bars)
                var markings = _options.Value.FieldMarkings;
                var lower = HsvToScalar(markings.Lower);
                var upper = HsvToScalar(markings.Upper);
                var threshImg = img.CvtColor(ColorConversionCodes.BGR2HSV)
                    .InRange(lower, upper);
                    /*
                    .GaussianBlur(new Size(_options.Value.BlurSize, _options.Value.BlurSize), 0)
                    .Dilate(0, null, (int)_options.Value.DilationIterations)
                    .Erode(0);*/

                var cannyImg = threshImg.Canny(1, 3);

                // Find and draw contours
                cannyImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);

                cannyImg = cannyImg.CvtColor(ColorConversionCodes.GRAY2BGR);
                cannyImg.DrawContours(contours, -1, new Scalar(0, 255, 255), 1);

                /*
                var rects = GetBoundingRects(contours);
                foreach (var rect in rects)
                {
                    cannyImg.Rectangle(rect, new Scalar(0, 0, 255), 2);
                }
                */

                return new Frame(threshImg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Frame(frame.Mat);
            }
        }

        private Rect[] GetBoundingRects(Point[][] contours)
        {
            var rects = new Rect[contours.Length];
            for (var i = 0; i < contours.Length; i++)
            {
                rects[i] = Cv2.BoundingRect(contours[i]);
            }

            return rects;
        }

        private Scalar HsvToScalar(HsvColor hsv)
        {
            return new Scalar()
            {
                Val0 = (int)(hsv.Hue / 360.0 * 179),
                Val1 = (int)(hsv.Saturation / 100.0 * 255),
                Val2 = (int)(hsv.Value / 100.0 * 255),
            };
        }
    }
}
