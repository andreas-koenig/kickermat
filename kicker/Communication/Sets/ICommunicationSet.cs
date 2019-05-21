namespace Communication.Sets
{
    using Calibration;
    using PlayerControl;
    using PluginSystem;

    /// <summary>
    /// Interface for a communication set which consits of a calibration interface and a player control interface.
    /// </summary>
    public interface ICommunicationSet : IKickerPlugin
    {
        /// <summary>
        /// Gets the player control.
        /// </summary>
        /// <value>The player control.</value>
        IPlayerControl PlayerControl { get; }

        /// <summary>
        /// Gets the calibration control.
        /// </summary>
        /// <value>The calibration control.</value>
        ICalibrationControl CalibrationControl { get; }

        /// <summary>
        /// Connects to the communication interface.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects the communication interface.
        /// </summary>
        void Disconnect();
    }
}