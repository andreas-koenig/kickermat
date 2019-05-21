namespace Calibration.Base
{
    using System.Windows.Forms;
    using Coach;

    /// <summary>
    /// User control for setting up the playing field size.
    /// </summary>
    public partial class PlayingFieldUserControl : UserControl
    {
        /// <summary>
        /// The settings to display in this user control.
        /// </summary>
        private readonly BasicCalibrationSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingFieldUserControl"/> class.
        /// </summary>
        /// <param name="settings">The used Settings.</param>
        /// <param name="updatelabels">Callback to update labels.</param>
        public PlayingFieldUserControl(BasicCalibrationSettings settings)
        {
            this.InitializeComponent();
            this.settings = settings;
            this.propertyGridPlayingField.SelectedObject = settings;
        }

        /// <summary>
        /// Updates the display values in this user control.
        /// </summary>
        public override void Refresh()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.Refresh));
            }
            else
            {
                base.Refresh();
                this.propertyGridPlayingField.Refresh();
            }
        }
    }
}