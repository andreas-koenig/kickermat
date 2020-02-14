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

        [NumberParameter("Blur Size", "The size of the WxH kernel for the gaussian blur.", 3, 3, 9, 2)]
        public double BlurSize { get; set; } = 3;

        [NumberParameter("Dilation Iterations", "The amount of iterations that the dilation operation is executed.",
            1, 0, 10, 1)]
        public double DilationIterations { get; set; } = 1;

        [NumberParameter("ContourHierarchy", "The contours of the image are organized in a hierarchy. This is their level.",
            10, 0, 20, 1)]
        public double ContourHierarchy { get; set; } = 10;
    }
}
