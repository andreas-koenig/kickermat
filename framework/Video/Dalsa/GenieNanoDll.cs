using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Video.Dalsa
{
    internal static class GenieNanoDll
    {
        // Path of to the C++ DLL
        private const string DalsaDll = "GenieNanoCamera.dll";

        // Delegates for callbacks that are passed to the DLL
        internal delegate void CameraConnected(string name);

        internal delegate void CameraDisconnected(string name);

        internal delegate void FrameArrived(int bufferIndex, IntPtr frameAddress);

        // Methods imported from the DLL
        [DllImport(DalsaDll, EntryPoint = "CreateCamera", CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateCamera(string name, RegionOfInterest roi,
            FrameArrived frameArrived, CameraConnected connected, CameraDisconnected disconnected);

        [DllImport(DalsaDll, EntryPoint = "DestroyCamera")]
        public static extern void DestroyCamera(IntPtr camera);

        [DllImport(DalsaDll, EntryPoint = "StartAcquisition")]
        public static extern bool StartAcquisition(IntPtr camera);

        [DllImport(DalsaDll, EntryPoint = "StopAcquisition")]
        public static extern bool StopAcquisition(IntPtr camera);

        [DllImport(DalsaDll, EntryPoint = "ReleaseBuffer")]
        public static extern bool ReleaseBuffer(IntPtr camera, int bufferIndex);

        [DllImport(DalsaDll, EntryPoint = "SetFeatureValue", CharSet = CharSet.Ansi)]
        public static extern bool SetFeatureValue(IntPtr camera, string featureName,
            double featureValue);
    }
}
