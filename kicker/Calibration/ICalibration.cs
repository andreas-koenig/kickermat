namespace Calibration
{
    using ObjectDetection;

    /// <summary>
    /// Interface which describes a calibration class.
    /// </summary>
    public interface ICalibration
    {
        /// <summary>
        /// Gets the calibration state.
        /// </summary>
        /// <value>The calibration state.</value>
        CalibrationState State { get; }

        /// <summary>
        /// Cancels this calibration.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Executes this calibration.
        /// </summary>
        void Execute();

        ICorrectionParams Params { get; }
    }
}
