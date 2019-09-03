using System;
using System.Collections.Generic;
using System.Text;
using Configuration;

namespace VideoSource.Dalsa
{
    public class DalsaSettings
    {
        // Parameter constants
        internal const double GAIN_DEFAULT = 1.0;
        internal const double GAIN_MIN = 1.0;
        internal const double GAIN_MAX = 8.0;
        internal const double EXPOSURE_TIME_DEFAULT = 15000.0;
        internal const double EXPOSURE_TIME_MIN = 1.0;
        internal const double EXPOSURE_TIME_MAX = 33246.0;

        public string CameraName { get; set; }

        [NumberParameter("Gain", "The analog gain (brightness)",
            GAIN_DEFAULT, GAIN_MIN, GAIN_MAX, 0.1)]
        public double Gain { get; set; }

        [NumberParameter("Exposure Time", "The exposure time in microseconds",
            EXPOSURE_TIME_DEFAULT, EXPOSURE_TIME_MIN, EXPOSURE_TIME_MAX, 10)]
        public double ExposureTime { get; set; }
    }
}
