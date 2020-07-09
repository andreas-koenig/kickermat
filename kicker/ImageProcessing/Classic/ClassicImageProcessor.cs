using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.Settings;
using Api.Settings.Parameter;
using ImageProcessing.OpenCvImageProcessor;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing
{
    public class ClassicImageProcessor : BaseVideoProcessor, IImageProcessor
    {
        private readonly IWriteable<ClassicImageProcessorSettings> _options;
        private readonly SoccerCourtDetector _courtDetector;
        private readonly BallDetector _ballDetector;

        // FPS
        private DateTime _lastFrameTime = DateTime.Now;
        private int _framesProcessed = 0;

        public ClassicImageProcessor(IVideoSource camera, ILogger<ClassicImageProcessor> logger,
            IWriteable<ClassicImageProcessorSettings> options)
            : base(camera, logger)
        {
            _options = options;
            _courtDetector = new SoccerCourtDetector(_options);
            _ballDetector = new BallDetector(_options);
            Channel = GetChannels().First();
        }

        public override IEnumerable<Channel> GetChannels()
        {
            return _courtDetector.GetChannels()
                .Union(_ballDetector.GetChannels());
        }

        protected override IFrame ProcessFrame(IFrame frame)
        {
            try
            {
                (var courtImg, var court) = _courtDetector.DetectSoccerCourt(frame?.Mat, Channel?.Id);
                (var ballImg, var ball) = _ballDetector.DetectBall(frame?.Mat, Channel?.Id);

                var img = courtImg == null ? ballImg : courtImg;
                img.Rectangle(ball, new Scalar(0, 255, 0), 2);

                _framesProcessed++;
                if ((DateTime.Now - _lastFrameTime).TotalSeconds >= 1)
                {
                    var fps = _framesProcessed.ToString();
                    img.PutText(fps, new Point(10, 10), HersheyFonts.HersheyPlain, 14, new Scalar(0, 255, 0), 1);
                    Console.WriteLine(fps);
                    _framesProcessed = 0;
                    _lastFrameTime = DateTime.Now;
                }

                return new Frame(img);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error: {0}", ex);
                return frame;
            }
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
