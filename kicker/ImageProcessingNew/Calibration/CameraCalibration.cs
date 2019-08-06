using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessingNew
{
    public class CameraCalibration : BaseVideoSource, ICameraCalibration
    {
        // Calibration constants
        private const int BOARD_WIDTH = 9;
        private const int BOARD_HEIGHT = 6;
        private const int SQUARE_SIZE = 50;
        private const int AMOUNT_FRAMES = 25;
        private static Size _boardSize = new Size(BOARD_WIDTH, BOARD_HEIGHT);

        // Calibration
        private object _objectLock;
        private EventHandler<CalibrationResult> _calibrationDone;
        private bool _calibrationRunning = false;
        private List<List<Point2f>> _chessboardCorners;

        // Video
        private readonly IVideoSource _videoSource;

        public CameraCalibration(IVideoSource videoSource)
        {
            _videoSource = videoSource;
            _objectLock = new object();
        }

        public void StartCalibration(EventHandler<CalibrationResult> calibrationDone)
        {
            lock (_objectLock)
            {
                _calibrationDone += calibrationDone;

                if (!_calibrationRunning)
                {
                    _calibrationRunning = true;
                    _videoSource.StartAcquisition(this);
                }
            }
        }

        public void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            // TODO: draw chessboard pattern and return new args to subscribed consumers
            args.Frame.Mat = FindChessboardCorners(args.Frame.Mat);

            if (_chessboardCorners.Count >= AMOUNT_FRAMES)
            {
                var result = CalculateDistortionParameters(args.Frame.Mat.Size());
                _calibrationRunning = false;
                _videoSource.StopAcquisition(this);
                _calibrationDone?.Invoke(this, result);
                _calibrationDone = null;
            }

            FrameArrived?.Invoke(this, args);
        }

        public void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            CameraDisconnected?.Invoke(this, args);
        }

        public void OnCameraConnected(object sender, CameraEventArgs args)
        {
            CameraConnected?.Invoke(this, args);
        }

        private Mat FindChessboardCorners(Mat frame)
        {
            var cornersOut = OutputArray.Create<Point2f>(new List<Point2f>());
            var found = Cv2.FindChessboardCorners(frame, _boardSize, cornersOut,
                ChessboardFlags.AdaptiveThresh |
                ChessboardFlags.FastCheck |
                ChessboardFlags.NormalizeImage);

            Console.WriteLine("Found Chessboard pattern: " + found);

            if (found)
            {
                var grayFrameOutput = OutputArray.Create(frame);
                Cv2.CvtColor(frame, grayFrameOutput, ColorConversionCodes.BGR2GRAY);

                var corners = (IEnumerable<Point2f>) cornersOut.GetVectorOfMat();
                var grayFrameInput = InputArray.Create(grayFrameOutput.GetMat());
                var correctedCorners = Cv2.CornerSubPix(
                    grayFrameInput, corners, new Size(11, 11), new Size(-1, -1),
                    new TermCriteria(CriteriaType.Eps | CriteriaType.MaxIter, 30, 0.1));

                _chessboardCorners.Add(new List<Point2f>(correctedCorners));

                Cv2.DrawChessboardCorners(frame, _boardSize, corners, found);
            }

            return frame;
        }

        private IEnumerable<Mat> calcBoardCornerPositions()
        {
            var corners = new List<Point3f>();
            for (int i = 0; i < BOARD_HEIGHT; ++i)
            {
                for (int j = 0; j < BOARD_WIDTH; ++j)
                {
                    var corner = new Point3f((float)j * SQUARE_SIZE, (float)i * SQUARE_SIZE, 0);
                    corners.Add(corner);
                }
            }

            return (IEnumerable<Mat>) corners;
        }

        private CalibrationResult CalculateDistortionParameters(Size imageSize)
        {
            Mat cameraMatrix = Mat.Eye(3, 3, MatType.CV_64F);
            cameraMatrix.Set(0, 0, 1.0);

            Mat distCoeffs = Mat.Zeros(new Size(8, 1), MatType.CV_64F);
            var objectPoints = calcBoardCornerPositions();
            Mat[] rVecs, tVecs;

            //objectPoints.resize(imagePoints.size(), objectPoints[0]);

            // Find intrinsic and extrinsic camera parameters
            double rms = Cv2.CalibrateCamera(objectPoints, (IEnumerable<Mat>)_chessboardCorners,
                imageSize, cameraMatrix, distCoeffs, out rVecs, out tVecs,
                CalibrationFlags.FixK4 | CalibrationFlags.FixK5);

            // TODO: do real calibration result
            return new CalibrationResult();
        }
    }
}
