using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoSource.Dalsa
{
    /// <summary>
    /// This class contains the entry points for the DLL created in the
    /// DalsaVideoSource project.
    /// </summary>
    internal abstract class DalsaApi
    {
        internal const string DALSA_DLL = @"..\DalsaVideoSource.dll";

        internal delegate void FrameArrivedDelegate(int index, IntPtr address);
        private static FrameArrivedDelegate _frameArrivedDelegate;

        internal delegate void ServerEventDelegate(string serverName);
        private static ServerEventDelegate _connectedDelegate;
        private static ServerEventDelegate _disconnectedDelegate;

        [DllImport(DALSA_DLL, EntryPoint = "startup")]
        private static extern void startup_dll(
            FrameArrivedDelegate frameArrived,
            ServerEventDelegate serverConnected,
            ServerEventDelegate serverDisconnected);

        /// <summary>
        /// Initialize the API. Has to be called before any acquisition is done or
        /// the list of cameras is queried.
        /// </summary>
        /// <param name="frameArrived">A function called when a new frame arrived</param>
        /// <param name="connectedCallback">Called when a camera is reconnected</param>
        /// <param name="disconnectedCallback">Called when a camera is disconnected</param>
        internal static void startup(
            FrameArrivedDelegate frameArrived,
            ServerEventDelegate connectedCallback,
            ServerEventDelegate disconnectedCallback)
        {
            _frameArrivedDelegate = frameArrived;
            _connectedDelegate = connectedCallback;
            _disconnectedDelegate = disconnectedCallback;
            startup_dll(_frameArrivedDelegate, _connectedDelegate, _disconnectedDelegate);
        }

        [DllImport(DALSA_DLL)]
        internal static extern void shutdown();

        /// <summary>
        /// Get the available cameras
        /// </summary>
        /// <returns>An array containing the camera names</returns>
        [DllImport(DALSA_DLL)]
        internal static extern void get_available_cameras();

        /// <summary>
        /// Start the acquisition.
        /// </summary>
        [DllImport(DALSA_DLL)]
        internal static extern bool start_acquisition(string camera_name);

        /// <summary>
        /// Stop the acquisition.
        /// </summary>
        [DllImport(DALSA_DLL)]
        internal static extern void stop_acquisition();

        /// <summary>
        /// Release the buffer with the specified index.
        /// </summary>
        /// <param name="index">The index of the buffer</param>
        [DllImport(DALSA_DLL)]
        internal static extern void release_buffer(int index);

        [DllImport(DALSA_DLL)]
        internal static extern bool set_feat_value(string camera_name, string feature_name,
            double value);

        [DllImport(DALSA_DLL)]
        internal static extern unsafe bool get_feat_value(string camera_name, string feature_name,
            double* value);
    }
}
