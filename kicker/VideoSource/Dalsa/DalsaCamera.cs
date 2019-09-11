using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace VideoSource.Dalsa
{
    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CameraConnected(string name);
    internal delegate void CameraDisconnected(string name);
    internal delegate void FrameArrived(int bufferIndex, IntPtr frameAddress);

    public class DalsaCamera : BaseVideoSource, IConfigurable<DalsaSettings>, IDisposable
    {
        // native DLL bindings
        internal const string DALSA_DLL = @"..\DalsaVideoSource.dll";

        private static FrameArrived _frameArrivedDelegate;
        private static CameraConnected _connectedDelegate;
        private static CameraDisconnected _disconnectedDelegate;

        [DllImport(DALSA_DLL, EntryPoint = "CreateCamera")]
        private static extern IntPtr DLL_CreateCamera(string name, FrameArrived frameArrived,
            CameraConnected connected, CameraDisconnected disconnected);

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
        private bool _disposed = false;
        private readonly object _lockObject = new object();
        private static DalsaCamera _this;
        private uint _count = 0;

        public DalsaCamera(ILogger<DalsaCamera> logger, IWritableOptions<DalsaSettings>
            options) : base(logger)
        {
            Options = options;
            _name = options.Value.CameraName;
            _this = this;
            _frameArrivedDelegate = FrameArrived;
            _connectedDelegate = CameraConnected;
            _disconnectedDelegate = CameraDisconnected;

            CreateCamera();
        }

        private void CreateCamera()
        {
            lock (_lockObject)
            {
                _cameraPtr = DLL_CreateCamera(_name, _frameArrivedDelegate,
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
            lock (_this?._lockObject)
            {
                if (!_this._acquisitionRunning)
                {
                    return;
                }

                var bayerMat = new Mat(1024, 1280, MatType.CV_8U, frameAddress);
                var bgrMat = bayerMat.CvtColor(ColorConversionCodes.BayerBG2BGR);
                bayerMat.Dispose();
                DLL_ReleaseBuffer(_this._cameraPtr, bufferIndex);

                var frame = new Frame(bgrMat);
                HandleFrameArrived(new FrameArrivedArgs(frame));

                // Manual garbage collection as this is called from unmanaged code
                _count += 1;
                if (_count % 100 == 0)
                {
                    GC.Collect();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            DLL_DestroyCamera(_cameraPtr);

            _disposed = true;
        }
    }
}
