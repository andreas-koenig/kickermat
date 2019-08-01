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
        const string DALSA_DLL = @"..\DalsaVideoSource.dll";

        /// <summary>
        /// Start the acquisition.
        /// </summary>
        /// <param name="callback">A function that gets called with the address
        /// and index of the buffer whenever a new frame is available.</param>
        [DllImport(DALSA_DLL)]
        internal static extern void start_acquisition(CallbackDelegate callback);
        
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

        internal delegate void CallbackDelegate(int index, IntPtr address);
    }
}
