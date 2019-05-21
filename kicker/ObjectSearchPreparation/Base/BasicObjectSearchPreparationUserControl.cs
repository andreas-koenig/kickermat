namespace ObjectSearchPreparation.Base
{
    using System.Windows.Forms;

    /// <summary>
    /// User control for displaying the setttings controls of an object search instance.
    /// </summary>
    public partial class BasicObjectSearchPreparationUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicObjectSearchPreparationUserControl"/> class.
        /// </summary>
        public BasicObjectSearchPreparationUserControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Adds the new control.
        /// </summary>
        /// <param name="newControl">The new control.</param>
        public void AddNewControl(Control newControl)
        {
            this.panelModuleSpecificControls.Controls.Add(newControl);
            newControl.Dock = DockStyle.Top;
        }
    }
}
