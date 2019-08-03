namespace Communication.Sets
{
  using System.Windows.Forms;
  using Communication.Calibration;
  using Communication.PlayerControl;

  /// <summary>
  /// Dummy communication set for software development without the Kicker.
  /// </summary>
  public class DummyCommunicationSet : ICommunicationSet
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DummyCommunicationSet"/> class.
    /// </summary>
    public DummyCommunicationSet()
    {
      this.CalibrationControl = new DummyCalibration();
      this.PlayerControl = new DumymPlayerControl();
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
        // do nothing
    }

    /// <summary>
    /// Disconnects the communication interface.
    /// </summary>
    public void Disconnect()
    {
        // do nothing
    }
  }
}
