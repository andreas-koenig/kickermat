using System;
using System.Collections.Generic;
using System.Text;
using Configuration.Parameter;

namespace ImageProcessing
{
    [KickermatSettings(typeof(ClassicImageProcessorSettings), "ImageProcessing")]
    public class ClassicImageProcessorSettings
    {
        [ColorRangeParameter("Field Markings", "A color range for the field markings.",
            0, 0, 220, 255, 120, 255)]
        public ColorRange FieldMarkings { get; set; } = new ColorRange(0, 0, 0, 0, 0, 0);

        [NumberParameter("Minimal BBox Area", "The minimal area a bounding box can have", 10, 1, 1000, 1)]
        public double MinimalBboxArea { get; set; } = 10;

        [ColorRangeParameter("Ball Color", "A color range for the ball.",
            32, 45, 60, 44, 60, 100)]
        public ColorRange BallColor { get; set; } = new ColorRange(0, 0, 0, 0, 0, 0);
    }
}
