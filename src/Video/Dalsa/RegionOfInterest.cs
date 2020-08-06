using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Video.Dalsa
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RegionOfInterest
    {
#pragma warning disable SX1309 // Due to C++ compatibility
#pragma warning disable SA1306
        public int XMin;
        public int YMin;
        public int Width;
        public int Height;
#pragma warning restore SX1309
#pragma warning restore SA1306

        public RegionOfInterest(int xMin, int yMin, int width, int height)
        {
            XMin = xMin;
            YMin = yMin;
            Width = width;
            Height = height;
        }
    }
}
