using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoSource.Dalsa
{
    class DalsaVideoSource : IVideoSource
    {
        private static int count = 0;

        public void StartAcquisition()
        {
            DalsaApi.start_acquisition(CreateMat);
        }

        public void StopAcquisition()
        {
            DalsaApi.stop_acquisition();
        }

        private static void CreateMat(int index, IntPtr address)
        {
            // Debayering of monochrome image
            Mat colorMat = new Mat();
            var monoMat = new Mat(1024, 1280, MatType.CV_8U, address);
            Cv2.CvtColor(monoMat, colorMat, ColorConversionCodes.BayerBG2BGR);

            var frame = new DalsaFrame(colorMat);
            DalsaApi.release_buffer(index);

            // TODO: Pass frame to ImagePreprocessor
        }
    }
}
