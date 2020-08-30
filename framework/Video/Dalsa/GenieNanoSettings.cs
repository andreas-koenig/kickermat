using System;
using System.Collections.Generic;
using System.Text;
using Api.Settings;
using Api.Settings.Parameter;

namespace Video.Dalsa
{
    public class GenieNanoSettings : ISettings
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

        [NumberParameter("Brightness", "The Genie Nano C1280 camera is configured to automatically adjust its brightness to the ambient lighting conditions. You can configure a target value, though.", BrightnessDefault, BrightnessMin, BrightnessMax, 1)]
        public double Brightness { get; set; }
    }
}
