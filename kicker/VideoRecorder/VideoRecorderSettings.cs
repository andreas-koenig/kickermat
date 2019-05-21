namespace VideoRecorder
{
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Settings of the default video recorder.
    /// </summary>
    public sealed class VideoRecorderSettings
    {
        /// <summary>
        /// Gets or sets the last used directory.
        /// </summary>
        public string LastDirectory { get; set; }

        /// <summary>
        /// Gets or sets the frames per second.
        /// </summary>
        public int FramesPerSecond { get; set; } = 30;

        public Size FrameSize { get; set; } = new Size(640, 480);

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            if (this.LastDirectory == null ||
                this.LastDirectory.Equals(string.Empty) ||
                !Directory.Exists(this.LastDirectory))
            {
                this.LastDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }

            if (this.FramesPerSecond <= 0 || this.FramesPerSecond > 100)
            {
                this.FramesPerSecond = 100;
            }
        }
    }
}
