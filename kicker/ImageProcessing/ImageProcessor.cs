using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing
{
    [VideoSource("imageprocessor", "Threshhold", "Edge Filter", "Edge Filter with Boxes")]
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
                var close_kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(_options.Value.DilationIterations, _options.Value.DilationIterations));

                // Mask white lines (includes bars)
                var markings = _options.Value.FieldMarkings;
                var lower = HsvToScalar(markings.Lower);
                var upper = HsvToScalar(markings.Upper);
                var threshImg = img.CvtColor(ColorConversionCodes.BGR2HSV)
                    .InRange(lower, upper);/*
                    .GaussianBlur(new Size(_options.Value.BlurSize, _options.Value.BlurSize), 0)
                    .MorphologyEx(MorphTypes.Close, close_kernel);*/

                // Find and draw contours
                //var edgeImg = threshImg.Sobel(threshImg.Type(), 1, 0);
                var edgeImg = threshImg.Canny(1, 3);
                
                edgeImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);
                edgeImg = edgeImg.CvtColor(ColorConversionCodes.GRAY2BGR);
                //edgeImg.DrawContours(contours, -1, new Scalar(0, 255, 255), 1);

                var rects = GetBoundingRects(contours);
                var avgArea = rects.Sum(rect => Area(rect)) / rects.Count();
                var avgAspectRatio = rects.Sum(rect => AspectRatio(rect)) / rects.Count();
                var longRects = rects.Where(rect => AspectRatio(rect) >= avgAspectRatio);
                var bigRects = rects.Where(rect => Area(rect) >= avgArea);
                foreach (var rect in longRects)
                {
                    edgeImg.Rectangle(rect, new Scalar(0, 0, 255), 2);
                }

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

        private int Area(Rect rect)
        {
            return rect.Width * rect.Height;
        }

        private float AspectRatio(Rect rect)
        {
            return ((float)rect.Width) / rect.Height;
        }
    }
}
