using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Camera;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace Kickermat.Services
{
    public class CameraCalibrationService : IObserver<IFrame>
    {
        // Constants
        private const int BoardWidth = 9;
        private const int BoardHeight = 6;
        private const int SquareSize = 50;
        private const int AmountFrames = 25;
        private const int SkipFramesCount = 20;
        private static readonly Size _boardSize = new Size(BoardWidth, BoardHeight);

        private readonly ILogger _logger;

        private IDisposable _subscription;
        private Action<int> _setProgress;
        private Action _finish;
        private CancellationToken _cancellationToken;

        // Calibration
        private readonly ConcurrentQueue<Point2f[]> _chessboardCorners;
        private bool _isCalibrationDone = false;
        private int _skippedFrames = 0;

        public CameraCalibrationService(ILogger<CameraCalibrationService> logger)
        {
            _logger = logger;
            _chessboardCorners = new ConcurrentQueue<Point2f[]>();
        }

        public void StartCalibration(
            ICamera<IFrame> camera, Action<int> updateProgress, Action finish,
            CancellationToken cancellationToken)
        {
            _setProgress = updateProgress;
            _finish = finish;
            _cancellationToken = cancellationToken;
            _subscription = camera.Subscribe(this);
        }

        public void OnCompleted()
        {
            _logger.LogInformation("Camera subscription completed");
        }

        public void OnError(Exception error)
        {
            _logger.LogError("Camera encountered an error: {Error}", error);
        }

        public async void OnNext(IFrame frame)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                _subscription.Dispose();
                _isCalibrationDone = true;
                return;
            }

            _skippedFrames = ++_skippedFrames % SkipFramesCount;
            if (_skippedFrames != 1 || _isCalibrationDone)
            {
                return;
            }

            // Compute camera matrix after all chessboard corners are collected
            if (_chessboardCorners.Count > AmountFrames)
            {
                _isCalibrationDone = true;
                _subscription.Dispose();
                ComputeCameraMatrix(out var cameraMatrix, out var distCoeffs, _boardSize);
                _finish();
            }

            // Collect ChessboardCorners
            var cornersTask = new Task(() => FindCorners(frame.ToBytes()), TaskCreationOptions.LongRunning);
            cornersTask.Start();
            await cornersTask;

            var cornersCount = _chessboardCorners.Count;
            var progress = (int)(((float)cornersCount) / AmountFrames);
            _setProgress(progress);
        }

        protected void FindCorners(byte[] frame)
        {
            var mat = Cv2.ImDecode(frame, ImreadModes.Color);

            var corners = new Mat<Point2f>();
            var cornersFound = Cv2.FindChessboardCorners(
                mat, _boardSize, corners,
                ChessboardFlags.AdaptiveThresh |
                ChessboardFlags.FastCheck |
                ChessboardFlags.NormalizeImage);

            if (!cornersFound)
            {
                return;
            }

            using (var grayFrame = new Mat())
            {
                Cv2.CvtColor(mat, grayFrame, ColorConversionCodes.BGR2GRAY);

                var correctedCorners = Cv2.CornerSubPix(
                    grayFrame, corners, new Size(11, 11), new Size(-1, -1),
                    new TermCriteria(CriteriaType.Eps | CriteriaType.MaxIter, 30, 0.1));

                _chessboardCorners.Enqueue(correctedCorners);
            }

            mat.Dispose();
        }

        private void ComputeCameraMatrix(out Mat cameraMatrix, out Mat distCoeffs, Size size)
        {
            cameraMatrix = new Mat(Mat.Eye(3, 3, MatType.CV_64FC1));
            distCoeffs = new Mat<double>();
            var objectPoints = new List<Mat>();
            var imagePoints = new List<Mat>();
            var cornerPositions = CalcBoardCornerPositions();

            var corners = _chessboardCorners.ToArray();
            for (int i = 0; i < _chessboardCorners.Count; i++)
            {
                objectPoints.Add(Mat.FromArray(cornerPositions));
                imagePoints.Add(Mat.FromArray(corners[i]));
            }

            double error = Cv2.CalibrateCamera(objectPoints, imagePoints,
                size, cameraMatrix, distCoeffs, out var rotationVectors, out var translationVectors,
                CalibrationFlags.FixK4 | CalibrationFlags.FixK5);

            _logger.LogInformation($"Calculated camera matrix with reprojection error: {error}");
        }

        private static IEnumerable<Point3f> CalcBoardCornerPositions()
        {
            for (int i = 0; i < BoardHeight; ++i)
            {
                for (int j = 0; j < BoardWidth; ++j)
                {
                    yield return new Point3f((float)j * SquareSize, (float)i * SquareSize, 0);
                }
            }
        }
    }
}
