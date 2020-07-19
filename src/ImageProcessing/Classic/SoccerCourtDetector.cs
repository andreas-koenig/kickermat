using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Settings;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing
{
    internal class SoccerCourtDetector
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

        private readonly IWriteable<ClassicImageProcessorSettings> _options;

        public SoccerCourtDetector(IWriteable<ClassicImageProcessorSettings> options)
        {
            _options = options;
        }

        public IEnumerable<Channel> GetChannels()
        {
            return new Channel[]
            {
                new Channel(ChannelThreshold, "Threshold Image", "The binarized image"),
                new Channel(ChannelBoxes, "Bounding Boxes", "The edge image with bounding boxes displayed"),
                new Channel(ChannelDetections, "Detections", "The detected touchline segements and corners"),
                new Channel(ChannelField, "Playing Field", "The detected playing field"),
            };
        }

        public (Mat, int) DetectSoccerCourt(Mat img, string channel)
        {
            try
            {
                // Mask white lines (includes bars)
                var markings = _options.Value.FieldMarkings;
                var lower = ClassicImageProcessor.HsvToScalar(markings.Lower);
                var upper = ClassicImageProcessor.HsvToScalar(markings.Upper);
                var threshImg = img.CvtColor(ColorConversionCodes.BGR2HSV)
                    .InRange(lower, upper);

                // Find contours
                threshImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);

                // Get bboxes and filter out the small ones that are irrelevant
                var boundingBoxes = ClassicImageProcessor.GetBoundingRects(contours)
                    .Where(rect => Area(rect) > _options.Value.MinimalBboxArea);

                // Get soccer court elements
                var corners = GetCorners(boundingBoxes);
                var touchlines = GetTouchlines(boundingBoxes);
                var goals = GetGoals(boundingBoxes);
                var halfWayLines = GetHalfWayLine(threshImg.Size(), boundingBoxes);

                threshImg = threshImg.CvtColor(ColorConversionCodes.GRAY2BGR);

                switch (channel)
                {
                    case ChannelThreshold:
                        return (threshImg, 0);

                    case ChannelBoxes:
                        boundingBoxes.ToList().ForEach(rect => threshImg.Rectangle(rect, _red, 2));
                        return (threshImg, 0);

                    case ChannelDetections:
                        touchlines.Item1.ForEach(rect => threshImg.Rectangle(rect, _red, 2));
                        touchlines.Item2.ForEach(rect => threshImg.Rectangle(rect, _red, 2));
                        new List<Point> { corners.Item1, corners.Item2, corners.Item3, corners.Item4 }
                            .ForEach(p => threshImg.Rectangle(new Rect(p.X - 3, p.Y - 3, 6, 6), _red, 2));
                        new List<Rect> { goals.Item1, goals.Item2 }
                            .ForEach(rect => threshImg.Rectangle(rect, _green, 2));
                        new List<Rect> { halfWayLines.Item1, halfWayLines.Item2 }
                            .ForEach(rect => threshImg.Rectangle(rect, _blue, 2));

                        return (threshImg, 0);

                    case ChannelField:
                        var transform = GetPerspectiveTransform(halfWayLines, corners);
                        var transformedImg = new Mat();
                        Cv2.WarpPerspective(threshImg, transformedImg, transform, threshImg.Size());// new Size(120 * 5, 689 * 5));//threshImg.Size());
                        return (transformedImg, 0);

                    default:
                        return (null, 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (null, 0);
            }
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

            // Sort boxes by their distance from the horizontal center and take the three closest.
            // Then search for the boxes that are farthest from the vertical center and take two.
            boxes = boxes
                .OrderBy(rect => Math.Abs(xCenter - (rect.Left + (rect.Width / 2))))
                .Take(3)
                .OrderBy(rect => Math.Abs(yCenter - (rect.Top + (rect.Height / 2))))
                .TakeLast(2)
                .OrderBy(rect => rect.Y);

            var upperBox = boxes.ElementAt(0);
            var lowerBox = boxes.ElementAt(1);

            var areaUpper = (float)Area(upperBox);
            var areaLower = (float)Area(lowerBox);
            var areaDev = areaUpper < areaLower ? areaUpper / areaLower : areaLower / areaUpper;

            var arUpper = AspectRatio(upperBox);
            var arLower = AspectRatio(lowerBox);
            var aspectRatioDev = arUpper < arLower ? arUpper / arLower : arLower / arUpper;

            var xCenterDiff = Math.Abs(upperBox.Left + (upperBox.Width / 2)
                - (lowerBox.Left + (lowerBox.Width / 2)));

            // Validate detected boxes: Must have similar area, aspect ratio and be at same X-coord.
            if (areaDev > 0.9 && aspectRatioDev > 0.9 && xCenterDiff < 20)
            {
                return (boxes.ElementAt(0), boxes.ElementAt(1));
            }

            return (new Rect(), new Rect());
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
            var factor = 9;

            (var topHalfWay, var bottomHalfWay) = halfWayLines;
            (var topLeft, var topRight, var bottomRight, var bottomLeft) = corners;

            int yTop = topHalfWay.Top;
            int yBottom = bottomHalfWay.Bottom;

            var middleTop = new Point(topHalfWay.Left + (topHalfWay.Width / 2), yTop);
            var middleBottom = new Point(bottomHalfWay.Left + (bottomHalfWay.Width / 2), yBottom);

            var origPoints = new Point2f[]
            {
                topLeft,
                middleTop,
                topRight,
                bottomRight,
                middleBottom,
                bottomLeft,
            };
            var newPoints = new Point2f[]
            {
                /*
                new Point(topLeft.X, yTop),
                new Point(topRight.X, yTop),
                new Point(bottomRight.X, yBottom),
                new Point(bottomLeft.X, yBottom),
                */
                new Point(0, 0),
                new Point(60 * factor, 0),
                new Point(120 * factor, 0),
                new Point(120 * factor, 68 * factor),
                new Point(60 * factor, 68 * factor),
                new Point(0, 68 * factor),
            };

            return Cv2.GetPerspectiveTransform(origPoints, newPoints);
        }
    }
}
