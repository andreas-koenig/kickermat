#if CAN_ENABLED
namespace Communication.Sets
{
    using System.Windows.Forms;
    using Calibration;
    using Calibration.CanCanGateway;
    using PlayerControl;
    using PlayerControl.CanCanGateway;

    /// <summary>
    /// Communication set for using a CAN-CAN-Gateway.
    /// </summary>
    public class CanCanGatwaySet : ICommunicationSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanCanGatwaySet"/> class.
        /// </summary>
        public CanCanGatwaySet()
        {
            this.CalibrationControl = new CanCanGatewayCalibration();
            this.PlayerControl = new CanCanGatewayPlayerControl();
        }

        /// <summary>
        /// Gets the player control.
        /// </summary>
        /// <value>The player control.</value>
        public IPlayerControl PlayerControl { get; private set; }

        /// <summary>
        /// Gets the calibration control.
        /// </summary>
        /// <value>The calibration control.</value>
        public ICalibrationControl CalibrationControl { get; private set; }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl
        {
            get { return null; }
        }

        /// <summary>
        /// Connects to the communication interface.
        /// </summary>
        public void Connect()
        {
        }

        /// <summary>
        /// Disconnects the communication interface.
        /// </summary>
        public void Disconnect()
        {
        }
    }
}
#endif