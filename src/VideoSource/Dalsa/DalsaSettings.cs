using System;
using System.Collections.Generic;
using System.Text;
using Api.Settings;
using Api.Settings.Parameter;

namespace VideoSource.Dalsa
{
    [KickermatSettings(typeof(DalsaCamera), "Camera", "Dalsa")]
    public class DalsaSettings : ISettings
    {
        // Parameter constants
        internal const int BrightnessDefault = 100;
        internal const int BrightnessMin = 0;
        internal const int BrightnessMax = 255;
        internal const double ExposureTimeDefault = 15000.0;
        internal const double ExposureTimeMin = 1.0;
        internal const double ExposureTimeMax = 33246.0;

        public string Name => "Dalsa Settings";

        public string CameraName { get; set; } = "Genie Nano C1280";

        [NumberParameter("Brightness", "The brightness of the camera",
            BrightnessDefault, BrightnessMin, BrightnessMax, 1)]
        public double Brightness { get; set; }
    }
}
