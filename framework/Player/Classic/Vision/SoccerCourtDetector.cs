using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Settings;
using Api.Settings.Parameter;
using OpenCvSharp;
using Video;

namespace Player.Classic.Vision
{
    internal class SoccerCourtDetector
    {
        private const int TouchlineMaxYDeviation = 20;
        private const int TouchLineSegmentMinAspectRatio = 15;

        public Rect DetectSoccerCourt(Mat img, ColorRange fieldMarkingsColor, int minBboxArea)
        {
            // Mask white lines (includes bars)
            var lower = ClassicImageProcessor.HsvToScalar(fieldMarkingsColor.Lower);
            var upper = ClassicImageProcessor.HsvToScalar(fieldMarkingsColor.Upper);
            var threshImg = img.CvtColor(ColorConversionCodes.BGR2HSV)
                .InRange(lower, upper);

            // Find contours
            threshImg.FindContours(out Point[][] contours, out HierarchyIndex[] hierarchy,
                RetrievalModes.External, ContourApproximationModes.ApproxTC89L1);

            // Get bboxes and filter out the small ones that are irrelevant
            var boundingBoxes = ClassicImageProcessor.GetBoundingRects(contours)
                .Where(rect => Area(rect) > minBboxArea);

            // Get soccer court elements
            var corners = GetCorners(boundingBoxes);
            var touchlines = GetTouchlines(boundingBoxes);
            var goals = GetGoals(boundingBoxes);
            var halfWayLines = GetHalfWayLine(threshImg.Size(), boundingBoxes);

            threshImg = threshImg.CvtColor(ColorConversionCodes.GRAY2BGR);

            return new Rect();
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
