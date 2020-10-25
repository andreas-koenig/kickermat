using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Motor
{
    internal class CanInterfaceDll
    {
        private const string CanDll = "CanInterface.dll";

        internal delegate void CalibrationDone(uint minPos, uint maxPos);

        [DllImport(CanDll, EntryPoint = "init", CharSet = CharSet.Ansi)]
        public static extern IntPtr Init();

        [DllImport(CanDll, EntryPoint = "destroy")]
        public static extern void Destroy(IntPtr apiHandle);

        [DllImport(CanDll, EntryPoint = "start_calibration")]
        public static extern void StartCalibration(IntPtr apiHandle, CalibrationDone doneCallback);

        [DllImport(CanDll, EntryPoint = "move_bar")]
        public static extern void MoveBar(IntPtr apiHandle, Bar bar, int position, int angle);
    }
}
