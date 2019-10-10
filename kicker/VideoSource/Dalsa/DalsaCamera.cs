using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace VideoSource.Dalsa
{
    internal delegate void CameraConnected(string name);

    internal delegate void CameraDisconnected(string name);

    internal delegate void FrameArrived(int bufferIndex, IntPtr frameAddress);

    [StructLayout(LayoutKind.Sequential)]
    internal struct RoiSettings
    {
        #pragma warning disable SX1309 // Due to C++ compatibility
        #pragma warning disable SA1306
        private int XMin;
        private int YMin;
        private int Width;
        private int Height;
        #pragma warning restore SX1309
        #pragma warning restore SA1306

        public RoiSettings(int xMin, int yMin, int width, int height)
        {
            XMin = xMin;
            YMin = yMin;
            Width = width;
            Height = height;
        }
    }

    public class DalsaCamera : BaseVideoSource
    {
        // native DLL bindings
        internal const string DalsaDll = @"..\DalsaVideoSource.dll";

        // constants
        private const int XMin = 64;
        private const int YMin = 184;
        private const int Width = 1184;
        private const int Height = 660;

        private readonly FrameArrived _frameArrivedDelegate;
        private readonly CameraConnected _connectedDelegate;
        private readonly CameraDisconnected _disconnectedDelegate;

        // members
        private readonly IWritableOptions<DalsaSettings> _options;

        private readonly object _lockObject = new object();
        private readonly string _name;
        private IntPtr _cameraPtr = IntPtr.Zero;
        private unsafe bool _acquisitionRunning;

        public DalsaCamera(
            ILogger<DalsaCamera> logger, IWritableOptions<DalsaSettings> options)
            : base(logger)
        {
            _options = options;
            _options.RegisterChangeListener(ApplyOptions);
            _name = options.Value.CameraName;

            _frameArrivedDelegate = FrameArrived;
            _connectedDelegate = CameraConnected;
            _disconnectedDelegate = CameraDisconnected;

            CreateCamera();
        }

        ~DalsaCamera()
        {
            if (!_cameraPtr.Equals(IntPtr.Zero))
            {
                DLL_DestroyCamera(_cameraPtr);
            }
        }

        protected override void StartAcquisition()
        {
            lock (_lockObject)
            {
                if (_cameraPtr.Equals(IntPtr.Zero))
                {
                    CreateCamera();
                }

                if (!DLL_StartAcquisition(_cameraPtr))
                {
                    var msg = string.Format("Dalsa Camera {0}: Failed to start acquisition", _name);
                    throw new VideoSourceException(msg);
                }

                _acquisitionRunning = true;
            }
        }

        protected override void StopAcquisition()
        {
            lock (_lockObject)
            {
                if (!_acquisitionRunning || _cameraPtr.Equals(IntPtr.Zero))
                {
                    return;
                }

                if (!DLL_StopAcquisition(_cameraPtr))
                {
                    var msg = string.Format("Dalsa Camera {0}: Failed to stop acquisition", _name);
                    throw new VideoSourceException(msg);
                }

                _acquisitionRunning = false;
            }
        }

        [DllImport(DalsaDll, EntryPoint = "CreateCamera")]
        private static extern IntPtr DLL_CreateCamera(string name, RoiSettings roi,
            FrameArrived frameArrived, CameraConnected connected, CameraDisconnected disconnected);

        [DllImport(DalsaDll, EntryPoint = "DestroyCamera")]
        private static extern void DLL_DestroyCamera(IntPtr camera);

        [DllImport(DalsaDll, EntryPoint = "StartAcquisition")]
        private static extern bool DLL_StartAcquisition(IntPtr camera);

        [DllImport(DalsaDll, EntryPoint = "StopAcquisition")]
        private static extern bool DLL_StopAcquisition(IntPtr camera);

        [DllImport(DalsaDll, EntryPoint = "ReleaseBuffer")]
        private static extern bool DLL_ReleaseBuffer(IntPtr camera, int bufferIndex);

        [DllImport(DalsaDll, EntryPoint = "SetFeatureValue")]
        private static extern bool DLL_SetFeatureValue(IntPtr camera, string featureName,
            double featureValue);

        private void CreateCamera()
        {
            lock (_lockObject)
            {
                var roi = new RoiSettings(XMin, YMin, Width, Height);
                _cameraPtr = DLL_CreateCamera(_name, roi, _frameArrivedDelegate,
                    _connectedDelegate, _disconnectedDelegate);
            }
        }

        private void ApplyOptions()
        {
            if (!DLL_SetFeatureValue(_cameraPtr, "autoBrightnessTarget", _options.Value.Brightness))
            {
                throw new VideoSourceException("Could not apply options to camera");
            }
        }

        private void CameraConnected(string name)
        {
            lock (_lockObject)
            {
                Console.WriteLine("CameraConnected");
                HandleConnect(new CameraEventArgs(name));
            }
        }

        private void CameraDisconnected(string name)
        {
            lock (_lockObject)
            {
                Console.WriteLine("CameraDisconnected");
                HandleDisconnect(new CameraEventArgs(name));
            }
        }

        private void FrameArrived(int bufferIndex, IntPtr frameAddress)
        {
            lock (_lockObject)
            {
                if (!_acquisitionRunning)
                {
                    return;
                }

                var bayerMat = new Mat(Height, Width, MatType.CV_8U, frameAddress);
                var bgrMat = bayerMat.CvtColor(ColorConversionCodes.BayerBG2BGR);
                bayerMat.Dispose();
                DLL_ReleaseBuffer(_cameraPtr, bufferIndex);

                var frame = new Frame(bgrMat);
                HandleFrameArrived(new FrameArrivedArgs(frame));
            }
        }
    }
}
