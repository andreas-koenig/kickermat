using System;
using System.Collections.Generic;
using System.Text;
using Configuration;

namespace ImageProcessing
{
    [KickerOptions(typeof(ImageProcessor), "ImageProcessing")]
    public class ImageProcessorSettings
    {
        [NumberParameter("Blur Size", "The size of the WxH kernel for the gaussian blur", 3, 3, 9, 1)]
        public double BlurSize { get; set; } = 3;

        [NumberParameter("Dilation Iterations", "The amount of iterations that the dilation operation is executed",
            1, 0, 10, 1)]
        public double DilationIterations { get; set; } = 1;

        [NumberParameter("ContourHierarchy", "The contours of the image are organized in a hierarchy. This is their level",
            10, 0, 20, 1)]
        public double ContourHierarchy { get; set; } = 10;
    }
}
