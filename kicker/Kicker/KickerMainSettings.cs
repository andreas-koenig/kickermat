namespace Kicker
{
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Class for Settings of <c>MainForm</c> which are not related to a certain plugin.
    /// </summary>
    public class KickerMainSettings
    {
        /// <summary>
        /// Gets or sets the directory from where a video was loaded the last time.
        /// </summary>
        public string LastVideoDirectory { get; set; }

        /// <summary>
        /// Gets or sets the directory from where a driver was loaded the last time.
        /// </summary>
        public string LastDriverDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path to the driver or video was load the last time.
        /// </summary>
        public string LastVideoOrDriver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread that closes CVB popup windows should be started.
        /// </summary>
        public bool StartCVBPopupKiller { get; set; }

        /// <summary>
        /// Gets or sets the top value of the screen position of the main window.
        /// </summary>
        [DefaultValueAttribute(50)]
        public int MainWindowTop { get; set; }

        /// <summary>
        /// Gets or sets the left value of the screen position of the main window.
        /// </summary>
        [DefaultValueAttribute(50)]
        public int MainWindowLeft { get; set; }

        /// <summary>
        /// Gets or sets the top value of the screen position of the image window.
        /// </summary>
        [DefaultValueAttribute(100)]
        public int ImageDisplayTop { get; set; }

        /// <summary>
        /// Gets or sets the left value of the screen position of the image window.
        /// </summary>
        [DefaultValueAttribute(100)]
        public int ImageDisplayLeft { get; set; }

        /// <summary>
        /// Gets or sets the selected tab in the main form.
        /// </summary>
        [DefaultValueAttribute(0)]
        public int SelectedTab { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            if (this.LastVideoDirectory == null ||
                this.LastVideoDirectory.Equals(string.Empty) ||
                !Directory.Exists(this.LastVideoDirectory))
            {
                this.LastVideoDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }

            //if (this.LastDriverDirectory == null ||
            //    this.LastDriverDirectory.Equals(string.Empty) ||
            //    !Directory.Exists(this.LastDriverDirectory))
            //{
            //    // Use CVB driver directory as default directory
            //    string cvbDirectory;
            //    Cvb.Utilities.GetCVBDirectory(out cvbDirectory, 255);
            //    this.LastDriverDirectory = cvbDirectory + "Drivers";
            //}
            
            if (this.LastVideoOrDriver == null ||
                this.LastVideoOrDriver.Equals(string.Empty) ||
                !File.Exists(this.LastVideoOrDriver))
            {
                this.LastVideoOrDriver = string.Empty;
            }

            bool mainposok = false;
            bool imageposok = false;

            // Check window positions on all screens
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                Rectangle w = Screen.AllScreens[i].WorkingArea;

                // calculate some inset
                if (w.Width >= 50) 
                {                             
                  w.Width -= 40;
                }

                if (w.Height >= 50) 
                {
                    w.Height -= 40;
                }

                // Main window has to be within any screen
                if (w.Contains(this.MainWindowLeft, this.MainWindowTop))
                {
                    mainposok = true;
                }

                // Display window has to be within any screen
                if (w.Contains(this.ImageDisplayLeft, this.ImageDisplayTop))
                {
                    imageposok = true;
                }
            }

            if (!mainposok)
            {
                this.MainWindowLeft = 50;
                this.MainWindowTop = 50;                
            }

            if (!imageposok)
            {
                this.ImageDisplayLeft = 100;
                this.ImageDisplayTop = 100;
            }           
        }
    }
}