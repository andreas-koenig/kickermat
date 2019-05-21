namespace Logging.log4net.Appenders
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// User control for displaying the messages of a list box appender.
    /// </summary>
    public partial class ListBoxAppenderUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxAppenderUserControl"/> class.
        /// </summary>
        public ListBoxAppenderUserControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the logging list box.
        /// </summary>
        /// <value>The logging list box.</value>
        public ListBox LoggingListBox
        {
            get
            {
                return this.listBoxLogMessages;
            }
        }

        /// <summary>
        /// Handles the Click event of the toolStripMenuItemClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            this.listBoxLogMessages.Items.Clear();
        }
    }
}
