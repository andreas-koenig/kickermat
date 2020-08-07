using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Settings;
using Api.Settings.Parameter;
using OpenCvSharp;

namespace Webapp.Player.Classic.Vision
{
    internal class ClassicImageProcessor
    {
        private readonly SoccerCourtDetector _courtDetector;
        private readonly BallDetector _ballDetector;

        // FPS
        private DateTime _lastFrameTime = DateTime.Now;
        private int _framesProcessed = 0;

        private readonly IWriteable<ClassicImageProcessorSettings> _settings;

        public ClassicImageProcessor(IWriteable<ClassicImageProcessorSettings> settings)
        {
            _settings = settings;
            _courtDetector = new SoccerCourtDetector();
            _ballDetector = new BallDetector();
        }

        public Mat ProcessFrame(Mat img)
        {
            var settings = _settings.Value;
            _courtDetector.DetectSoccerCourt(img, settings.FieldMarkings,
                (int)settings.MinimalBboxArea);
            var ball = _ballDetector.DetectBall(img, settings.BallColor);

            img.Rectangle(ball, new Scalar(0, 255, 0), 2);
            // TODO: Draw rect for playing field

            _framesProcessed++;
            if ((DateTime.Now - _lastFrameTime).TotalSeconds >= 1)
            {
                var fps = _framesProcessed.ToString();
                img.PutText(fps, new Point(10, 10), HersheyFonts.HersheyPlain, 14, new Scalar(0, 255, 0), 1);
                Console.WriteLine(fps);
                _framesProcessed = 0;
                _lastFrameTime = DateTime.Now;
            }

            return img;
        }

        internal static Scalar HsvToScalar(HsvColor hsv)
        {
            return new Scalar()
            {
                Val0 = (int)(hsv.Hue / 360.0 * 179),
                Val1 = (int)(hsv.Saturation / 100.0 * 255),
                Val2 = (int)(hsv.Value / 100.0 * 255),
            };
        }

        internal static IEnumerable<Rect> GetBoundingRects(Point[][] contours)
        {
            var rects = new Rect[contours.Length];
            for (var i = 0; i < contours.Length; i++)
            {
                rects[i] = Cv2.BoundingRect(contours[i]);
            }

            return rects;
        }
    }
}
