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
    public class ImageProcessor : BaseVideoProcessor, IImageProcessor
    {
        private const string ChannelThreshold = "threshold";
        private const string ChannelBoxes = "boxes";
        private const string ChannelDetections = "detections";
        private const string ChannelField = "field";

        private const int TouchlineMaxYDeviation = 20;
        private const int TouchLineSegmentMinAspectRatio = 15;

        private readonly Scalar _red = new Scalar(0, 0, 255);
        private readonly Scalar _yellow = new Scalar(255, 255, 0);
        private readonly Scalar _green = new Scalar(0, 255, 0);
        private readonly Scalar _blue = new Scalar(255, 0, 0);

        private readonly IWritableOptions<ImageProcessorSettings> _options;

        public ImageProcessor(IVideoSource camera, ILogger<ImageProcessor> logger,
            IWritableOptions<ImageProcessorSettings> options)
            : base(camera, logger)
        {
            _options = options;
            Channel = GetChannels().First();
        }

        public override IEnumerable<Channel> GetChannels()
        {
            return new Channel[]
            {
                new Channel(ChannelThreshold, "Threshold Image", "The binarized image"),
                new Channel(ChannelBoxes, "Bounding Boxes", "The edge image with bounding boxes displayed"),
                new Channel(ChannelDetections, "Detections", "The detected touchline segements and corners"),
                new Channel(ChannelField, "Playing Field", "The detected playing field"),
            };
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

                frame.Release();

                // Find contours
                threshImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);

                // Get bboxes and filter out the small ones that are irrelevant
                var boundingBoxes = GetBoundingRects(contours)
                    .Where(rect => Area(rect) > _options.Value.MinimalBboxArea);

                // Get soccer court elements
                var corners = GetCorners(boundingBoxes);
                var touchlines = GetTouchlines(boundingBoxes);
                var goals = GetGoals(boundingBoxes);
                var halfWayLines = GetHalfWayLine(threshImg.Size(), boundingBoxes);

                threshImg = threshImg.CvtColor(ColorConversionCodes.GRAY2BGR);

                switch (Channel?.Id)
                {
                    case ChannelThreshold:
                        return new Frame(threshImg);

                    case ChannelBoxes:
                        boundingBoxes.ToList().ForEach(rect => threshImg.Rectangle(rect, _red, 2));
                        return new Frame(threshImg);

                    case ChannelDetections:
                        touchlines.Item1.ForEach(rect => threshImg.Rectangle(rect, _yellow, 2));
                        touchlines.Item2.ForEach(rect => threshImg.Rectangle(rect, _yellow, 2));
                        new List<Point> { corners.Item1, corners.Item2, corners.Item3, corners.Item4 }
                            .ForEach(p => threshImg.Rectangle(new Rect(p.X - 3, p.Y - 3, 6, 6), _red, 2));
                        new List<Rect> { goals.Item1, goals.Item2 }
                            .ForEach(rect => threshImg.Rectangle(rect, _green, 2));
                        new List<Rect> { halfWayLines.Item1, halfWayLines.Item2 }
                            .ForEach(rect => threshImg.Rectangle(rect, _blue, 2));

                        return new Frame(threshImg);

                    case ChannelField:
                        var transform = GetPerspectiveTransform(halfWayLines, corners);
                        Cv2.WarpPerspective(threshImg, threshImg, transform, threshImg.Size());
                        return new Frame(threshImg);

                    default:
                        return new Frame(threshImg);
                }
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

        private int GetMiddle(Point p1, Point p2, Func<Point, int> f)
        {
            if (f(p1) > f(p2))
            {
                return f(p2) + (f(p1) - f(p2));
            }

            return f(p1) + (f(p2) - f(p1));
        }

        private (Point, Point, Point, Point) GetCorners(IEnumerable<Rect> boxes)
        {
            var leftRects = boxes
                    .OrderBy(rect => rect.X)
                    .Take(3)
                    .OrderBy(rect => rect.Y);
            var topLeft = leftRects.First().TopLeft;
            var bottomLeft = new Point(leftRects.Last().X, leftRects.Last().Bottom);

            var rightRects = boxes
                .OrderBy(rect => rect.Right)
                .TakeLast(3)
                .OrderBy(rect => rect.Y);
            var bottomRight = rightRects.Last().BottomRight;
            var topRight = new Point(rightRects.First().Right, rightRects.First().Top);

            return (topLeft, topRight, bottomRight, bottomLeft);
        }

        private (List<Rect>, List<Rect>) GetTouchlines(IEnumerable<Rect> boxes)
        {
            boxes = boxes
                .OrderBy(rect => rect.Y)
                .Where(rect => AspectRatio(rect) > TouchLineSegmentMinAspectRatio);

            var upperTouchline = GetTouchline(boxes);
            var lowerTouchline = GetTouchline(boxes.Reverse());

            return (upperTouchline, lowerTouchline);
        }

        private List<Rect> GetTouchline(IEnumerable<Rect> boxes)
        {
            int oldY = boxes.First().Y;
            return boxes
                .TakeWhile(rect =>
                {
                    bool valid = Math.Abs(rect.Y - oldY) < TouchlineMaxYDeviation;
                    oldY = rect.Y;
                    return valid;
                })
                .OrderBy(rect => rect.X)
                .ToList();
        }

        private (Rect, Rect) GetGoals(IEnumerable<Rect> boxes)
        {
            boxes = boxes.OrderBy(rect => rect.X);

            return (boxes.First(), boxes.Last());
        }

        private (Rect, Rect) GetHalfWayLine(Size imgSize, IEnumerable<Rect> boxes)
        {
            var xCenter = imgSize.Width / 2;
            var yCenter = imgSize.Height / 2;

            boxes = boxes
                .OrderBy(rect => Math.Abs(xCenter - (rect.Right - rect.Left)))
                .Take(3)
                .OrderBy(rect => Math.Abs(yCenter - (rect.Top - rect.Bottom)))
                .TakeLast(2)
                .OrderBy(rect => rect.Y);

            return (boxes.ElementAt(0), boxes.ElementAt(1));
        }

        private int Area(Rect rect)
        {
            return rect.Width * rect.Height;
        }

        private float AspectRatio(Rect rect)
        {
            return ((float)rect.Width) / rect.Height;
        }

        private Mat GetPerspectiveTransform((Rect, Rect) halfWayLines, (Point, Point, Point, Point) corners)
        {
            (var topHalfWay, var bottomHalfWay) = halfWayLines;
            (var topLeft, var topRight, var bottomRight, var bottomLeft) = corners;

            int yTop = topHalfWay.Top;
            int yBottom = bottomHalfWay.Bottom;

            var origPoints = new Point2f[] { topLeft, topRight, bottomRight, bottomLeft };
            var newPoints = new Point2f[]
            {
                new Point(topLeft.X, yTop),
                new Point(topRight.X, yTop),
                new Point(bottomRight.X, yBottom),
                new Point(bottomLeft.X, yBottom),
            };

            return Cv2.GetPerspectiveTransform(origPoints, newPoints);
        }
    }
}
