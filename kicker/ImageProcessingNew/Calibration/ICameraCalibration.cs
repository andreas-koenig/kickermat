using System;
using System.Collections.Generic;
using System.Text;
using VideoSource;

namespace ImageProcessingNew
{
    public interface ICameraCalibration : IVideoSource, IVideoConsumer
    {
        /// <summary>
        /// Start the calibration to calculate the parameters for the correction of the radial and
        /// tangential camera distortion. The calibration is only working when the acquisition is
        /// started.
        /// </summary>
        /// <param name="calibrationDone">A delegate that gets called when the calibration
        /// terminates.</param>
        void StartCalibration(EventHandler<CalibrationResult> calibrationDone);
    }

    public class CalibrationResult { }
}
