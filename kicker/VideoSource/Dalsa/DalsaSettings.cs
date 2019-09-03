using System;
using System.Collections.Generic;
using System.Text;
using Configuration;

namespace VideoSource.Dalsa
{
    public class DalsaSettings
    {
        // Parameter constants
        internal const int BRIGHTNESS_DEFAULT = 100;
        internal const int BRIGHTNESS_MIN = 0;
        internal const int BRIGHTNESS_MAX = 255;
        internal const double EXPOSURE_TIME_DEFAULT = 15000.0;
        internal const double EXPOSURE_TIME_MIN = 1.0;
        internal const double EXPOSURE_TIME_MAX = 33246.0;

        public string CameraName { get; set; }

        [NumberParameter("Brightness", "The brightness of the camera",
            BRIGHTNESS_DEFAULT, BRIGHTNESS_MIN, BRIGHTNESS_MAX, 1)]
        public double Brightness { get; set; }

        [NumberParameter("Exposure Time", "The exposure time in microseconds",
            EXPOSURE_TIME_DEFAULT, EXPOSURE_TIME_MIN, EXPOSURE_TIME_MAX, 10)]
        public double ExposureTime { get; set; }
    }
}
