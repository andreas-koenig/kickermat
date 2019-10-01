using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.BallSearch
{
    public class BallSearch : BaseVideoProcessor, IBallSearch
    {
        // Extracted with Paint.NET: H = [0, 360], S/V = [0, 100]
        // Normalize to OpenCV: H = [0, 180], S/V = [0, 255]
        private const int LOW_H = (int)((19.0 / 360) * 180);
        private const int HIGH_H = (int)((58.0 / 360) * 180);
        private const int LOW_S = (int)((64.0 / 100) * 255);
        private const int HIGH_S = (int)((85.0 / 100) * 255);
        private const int LOW_V = (int)((75.0 / 100) * 255);
        private const int HIGH_V = (int)((100.0 / 100) * 255);

        private IVideoSource _camera;
        public BallSearch(IVideoSource camera, ILogger<BallSearch> logger): base(camera, logger)
        {
            _camera = camera;
        }

        public void Start()
        {
            StartAcquisition();
        }

        protected override IFrame ProcessFrame(IFrame frame)
        {

            using (var img = frame.Mat)
            using (var hsvImg = img.CvtColor(ColorConversionCodes.BGR2HSV))
            {
                var lower = new Scalar(LOW_H, LOW_S, LOW_V);
                var upper = new Scalar(HIGH_H, HIGH_S, HIGH_V);
                var threshImg = hsvImg
                    .InRange(lower, upper)
                    .GaussianBlur(new Size(3, 3), 0)
                    .Dilate(0)
                    .Erode(0);

                Cv2.ImShow("Mask", threshImg);
                Cv2.WaitKey();

                var circles = Cv2.HoughCircles(threshImg, HoughMethods.Gradient, 2,
                    threshImg.Rows / 4, 100, 50, 3, 30);

                Console.WriteLine("{0} balls detected", circles.Length);
                foreach (var circle in circles)
                {
                    Console.WriteLine("Center: ({0}, {1}), Radius: {2}",
                        circle.Center.X, circle.Center.Y, circle.Radius);
                }

                threshImg.Release();
            }

            return frame;
        }

        protected override void StartAcquisition()
        {
            _camera.StartAcquisition(this);
        }

        protected override void StopAcquisition()
        {
            _camera.StopAcquisition(this);
        }
    }
}
