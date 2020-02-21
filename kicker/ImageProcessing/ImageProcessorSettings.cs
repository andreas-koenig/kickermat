using System;
using System.Collections.Generic;
using System.Text;
using Configuration;

namespace ImageProcessing
{
    [KickerOptions(typeof(ImageProcessor), "ImageProcessing")]
    public class ImageProcessorSettings
    {
        [ColorRangeParameter("Field Markings", "A color range for the field markings.",
            0, 0, 220, 255, 120, 255)]
        public ColorRange FieldMarkings { get; set; } = new ColorRange(0, 0, 0, 0, 0, 0);

        [NumberParameter("Minimal BBox Area", "The minimal area a bounding box can have", 10, 1, 1000, 1)]
        public double MinimalBboxArea { get; set; } = 10;
    }
}
