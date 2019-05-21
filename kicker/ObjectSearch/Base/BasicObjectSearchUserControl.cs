namespace ObjectSearch.Base
{
    using System.Windows.Forms;

    /// <summary>
    /// User control for managing the settings of an object search algorithm.
    /// </summary>
    public partial class BasicObjectSearchUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicObjectSearchUserControl"/> class.
        /// </summary>
        /// <param name="objectSearchPreparationUserControl">The image binarization user control.</param>
        /// <param name="objectSearchSettingsUserControl">The obect search settings user control.</param>
        public BasicObjectSearchUserControl(Control objectSearchPreparationUserControl, Control objectSearchSettingsUserControl)
        {
            this.InitializeComponent();
            objectSearchSettingsUserControl.Dock = DockStyle.Fill;
            this.tabPageObjectSearch.Controls.Add(objectSearchSettingsUserControl);
            objectSearchPreparationUserControl.Dock = DockStyle.Fill;
            this.tabPagePreparation.Controls.Add(objectSearchPreparationUserControl);
        }
    }
}
