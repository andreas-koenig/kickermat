using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace VideoSource.Dalsa
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RoiSettings
    {
        private int XMin;
        private int YMin;
        private int Width;
        private int Height;

        public RoiSettings(int xMin, int yMin, int width, int height)
        {
            XMin = xMin;
            YMin = yMin;
            Width = width;
            Height = height;
        }
    }

    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CameraConnected(string name);
    internal delegate void CameraDisconnected(string name);
    internal delegate void FrameArrived(int bufferIndex, IntPtr frameAddress);

    public class DalsaCamera : BaseVideoSource, IConfigurable<DalsaSettings>
    {
        // constants
        private const int X_MIN = 64;
        private const int Y_MIN = 184;
        private const int WIDTH = 1184;
        private const int HEIGHT = 660;

        // native DLL bindings
        internal const string DALSA_DLL = @"..\DalsaVideoSource.dll";

        private readonly FrameArrived _frameArrivedDelegate;
        private readonly CameraConnected _connectedDelegate;
        private readonly CameraDisconnected _disconnectedDelegate;

        [DllImport(DALSA_DLL, EntryPoint = "CreateCamera")]
        private static extern IntPtr DLL_CreateCamera(string name, RoiSettings roi,
            FrameArrived frameArrived, CameraConnected connected, CameraDisconnected disconnected);

        [DllImport(DALSA_DLL, EntryPoint = "DestroyCamera")]
        private static extern void DLL_DestroyCamera(IntPtr camera);

        [DllImport(DALSA_DLL, EntryPoint = "StartAcquisition")]
        private static extern bool DLL_StartAcquisition(IntPtr camera);

        [DllImport(DALSA_DLL, EntryPoint = "StopAcquisition")]
        private static extern bool DLL_StopAcquisition(IntPtr camera);

        [DllImport(DALSA_DLL, EntryPoint = "ReleaseBuffer")]
        private static extern bool DLL_ReleaseBuffer(IntPtr camera, int bufferIndex);

        [DllImport(DALSA_DLL, EntryPoint = "SetFeatureValue")]
        private static extern bool DLL_SetFeatureValue(IntPtr camera, string featureName,
            double featureValue);

        // members
        public IWritableOptions<DalsaSettings> Options { get; }

        private IntPtr _cameraPtr = IntPtr.Zero;
        private string _name;
        private unsafe bool _acquisitionRunning;
        private readonly object _lockObject = new object();

        public DalsaCamera(ILogger<DalsaCamera> logger, IWritableOptions<DalsaSettings>
            options) : base(logger)
        {
            Options = options;
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

        private void CreateCamera()
        {
            lock (_lockObject)
            {
                var roi = new RoiSettings(X_MIN, Y_MIN, WIDTH, HEIGHT);
                _cameraPtr = DLL_CreateCamera(_name, roi, _frameArrivedDelegate,
                    _connectedDelegate, _disconnectedDelegate);
            }
        }

        public void ApplyOptions()
        {
            if (!DLL_SetFeatureValue(_cameraPtr, "autoBrightnessTarget", Options.Value.Brightness))
            {
                throw new VideoSourceException("Could not apply options to camera");
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

                var bayerMat = new Mat(HEIGHT, WIDTH, MatType.CV_8U, frameAddress);
                var bgrMat = bayerMat.CvtColor(ColorConversionCodes.BayerBG2BGR);
                bayerMat.Dispose();
                DLL_ReleaseBuffer(_cameraPtr, bufferIndex);

                var frame = new Frame(bgrMat);
                HandleFrameArrived(new FrameArrivedArgs(frame));
            }
        }
    }
}
