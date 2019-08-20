﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace ImageProcessing.Calibration
{
    public class CameraCalibration : BaseVideoSource, ICameraCalibration
    {
        // Constants
        private const int BOARD_WIDTH = 9;
        private const int BOARD_HEIGHT = 6;
        private const int SQUARE_SIZE = 50;
        private const int AMOUNT_FRAMES = 25;
        private const int FRAME_BUFFER_SIZE = 10;
        private static Size _boardSize = new Size(BOARD_WIDTH, BOARD_HEIGHT);

        // Calibration
        private object _objectLock;
        private CalibrationDoneDelegate _calibrationDone;
        private ChessboardRecognizedDelegate _chessboardRecognized;
        private bool _isCalibrationRunning = false;
        private volatile bool _isFindingCorners = false;
        private readonly List<Point2f[]> _chessboardCorners;
        private RingBuffer<Mat> _frames;

        // Video
        private readonly IVideoSource _videoSource;

        public CameraCalibration(IVideoSource videoSource, ILogger<ICameraCalibration> logger)
            : base(logger)
        {
            _videoSource = videoSource;
            _objectLock = new object();
            _chessboardCorners = new List<Point2f[]>();
            _frames = new RingBuffer<Mat>(AMOUNT_FRAMES);
        }

        protected override void StartAcquisition()
        {
            Console.WriteLine("calib: startacq");

            if (FrameArrived.GetInvocationList().Length == 1 && !_isCalibrationRunning)
            {
                _videoSource.StartAcquisition(this);
                Logger.LogInformation("Acquisition started");
            }
        }

        protected override void StopAcquisition()
        {
            Console.WriteLine("calib: stopacq");

            if (FrameArrived == null && !_isCalibrationRunning)
            {
                _videoSource.StopAcquisition(this);
                Logger.LogInformation("Acquisition stopped");
            }
        }

        public void StartCalibration(
            CalibrationDoneDelegate calibrationDone,
            ChessboardRecognizedDelegate chessboardRecognized)
        {
            lock (_objectLock)
            {
                _calibrationDone += calibrationDone;
                _chessboardRecognized += chessboardRecognized;

                if (!_isCalibrationRunning)
                {
                    _isCalibrationRunning = true;
                    if (FrameArrived == null)
                    {
                        _videoSource.StartAcquisition(this);
                    }
                }
            }
        }

        public void AbortCalibration()
        {
            lock (_objectLock)
            {
                if (!_isCalibrationRunning)
                {
                    return;
                }

                _videoSource.StopAcquisition(this);
                _isCalibrationRunning = false;
                _calibrationDone = null;
                _chessboardRecognized = null;
            }
        }

        public void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            _frames.Add(args.Frame.Mat);
            if (!_isFindingCorners)
            {
                _ = FindChessboardCornersAsync(_frames.Take());
            }

            if (_isCalibrationRunning && _chessboardCorners.Count == AMOUNT_FRAMES)
            {
                if (FrameArrived == null)
                {
                    _videoSource.StopAcquisition(this);
                }

                lock (_objectLock)
                {
                    var result = CalculateDistortionParameters(args.Frame.Mat);
                    _isCalibrationRunning = false;
                    _calibrationDone(result);
                    _calibrationDone = null;
                    _chessboardRecognized = null;
                }
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

        private Task<Mat> FindChessboardCornersAsync(Mat frame)
        {
            return Task<Mat>.Run(() =>
            {
                _isFindingCorners = true;

                var corners = new MatOfPoint2f();
                var found = Cv2.FindChessboardCorners(frame, _boardSize, corners,
                    ChessboardFlags.AdaptiveThresh |
                    ChessboardFlags.FastCheck |
                    ChessboardFlags.NormalizeImage);

                if (found)
                {
                    var grayFrame = new Mat();
                    Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

                    var correctedCorners = Cv2.CornerSubPix(
                        grayFrame, corners, new Size(11, 11), new Size(-1, -1),
                        new TermCriteria(CriteriaType.Eps | CriteriaType.MaxIter, 30, 0.1));

                    Cv2.DrawChessboardCorners(frame, _boardSize,
                        (IEnumerable<Point2f>)correctedCorners, found);

                    lock (_objectLock)
                    {
                        _chessboardCorners.Add(correctedCorners);
                    }
                    _chessboardRecognized(_chessboardCorners.Count);
                }

                _isFindingCorners = false;

                return frame;
            });
        }

        private IEnumerable<Point3f> calcBoardCornerPositions()
        {
            for (int i = 0; i < BOARD_HEIGHT; ++i)
            {
                for (int j = 0; j < BOARD_WIDTH; ++j)
                {
                    yield return new Point3f((float)j * SQUARE_SIZE, (float)i * SQUARE_SIZE, 0);
                }
            }
        }

        private CalibrationResult CalculateDistortionParameters(Mat frame)
        {
            Mat cameraMatrix = new MatOfDouble(Mat.Eye(3, 3, MatType.CV_64FC1));
            Mat distCoeffs = new MatOfDouble();
            var objectPoints = new List<Mat>();
            var imagePoints = new List<Mat>();
            var cornerPositions = calcBoardCornerPositions();

            for (int i = 0; i < _chessboardCorners.Count; i++)
            {
                objectPoints.Add(MatOfPoint3f.FromArray(cornerPositions));
                imagePoints.Add(MatOfPoint2f.FromArray(_chessboardCorners[i]));
            }
            
            double error = Cv2.CalibrateCamera(objectPoints, imagePoints,
                frame.Size(), cameraMatrix, distCoeffs,
                out var rotationVectors, out var translationVectors,
                CalibrationFlags.FixK4 | CalibrationFlags.FixK5);

            var result = new CalibrationResult(cameraMatrix, distCoeffs, error);
            Logger.LogInformation("Calibration done:\n{}", result);

            return result;
        }
    }
}