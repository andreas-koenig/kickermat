namespace VideoRecorder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// User-Interface für die Einstellungen des Video-Recorders.
    /// </summary>
    public sealed partial class VideoRecorderUserControl : UserControl
    {
        /// <summary>
        /// The video recorder instance.
        /// </summary>
        private readonly VideoRecorder videoRecorder;

        private readonly BackgroundWorker _RecordingTimeUpdater = new BackgroundWorker();

        private readonly Stopwatch _RecordingStopwatch = new Stopwatch();
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoRecorderUserControl"/> class.
        /// </summary>
        /// <param name="recorder">The recorder.</param>
        public VideoRecorderUserControl(VideoRecorder recorder)
        {
            this.InitializeComponent();
            this.videoRecorder = recorder;
            this.videoRecorder.StateChanged += this.VideoRecorder_StateChanged;
            initSize();
            this.numericUpDownRecFramesPerSecond.Value = this.videoRecorder.Settings.FramesPerSecond;
            _RecordingTimeUpdater.WorkerSupportsCancellation = true;
            _RecordingTimeUpdater.DoWork += _RecordingTimeUpdater_DoWork;
        }

        private void _RecordingTimeUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            while (videoRecorder.Recording)
            {
                string text = "Recording time: " + _RecordingStopwatch.Elapsed;
                BeginInvoke(new MethodInvoker(() => labelRecordingTime.Text = text));
                Thread.Sleep(100);
            }
        }

        private void initSize()
        {
            if (videoRecorder.Settings.FrameSize.Height <= 1080 && videoRecorder.Settings.FrameSize.Height > 0)
                numericUpDownHeight.Value = videoRecorder.Settings.FrameSize.Height;
            else
                numericUpDownHeight.Value = videoRecorder.Settings.FrameSize.Height > 1080 ? 1080 : 1;
            if (videoRecorder.Settings.FrameSize.Width <= 1920 && videoRecorder.Settings.FrameSize.Width > 0)
                numericUpDownWidth.Value = videoRecorder.Settings.FrameSize.Width;
            else
                numericUpDownWidth.Value = videoRecorder.Settings.FrameSize.Width > 1920 ? 1920 : 1;
        }

        /// <summary>
        /// Gets the video recorder.
        /// </summary>
        /// <value>The video recorder.</value>
        public UserControl VideoRecorderControl
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the video file.
        /// </summary>
        /// <value>The name of the video file.</value>
        public string VideoFileName
        {
            get
            {
                return this.labelFileName.Text;
            }

            set
            {
                this.labelFileName.Text = value;
                this.controlToolTips.SetToolTip(this.labelFileName, value);
            }
        }

        /// <summary>
        /// Handles the StateChanged event of the VideoRecorder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="VideoRecorderStateChangedEventArgs"/> instance containing the event data.</param>
        private void VideoRecorder_StateChanged(object sender, VideoRecorderStateChangedEventArgs e)
        {
            switch (e.NewState)
            {
                case VideoRecorderState.ImageLoaded:
                    this.buttonInit.Enabled = true;
                    break;
                case VideoRecorderState.RecordingInitialized:
                    this.buttonRecord.Enabled = true;
                    break;
                case VideoRecorderState.RecordingStarted:
                    this.buttonInit.Enabled = false;
                    this.buttonRecord.Enabled = false;
                    this.buttonStop.Enabled = true;
                    this.numericUpDownRecFramesPerSecond.Enabled = false;
                    _RecordingStopwatch.Restart();
                    _RecordingTimeUpdater.RunWorkerAsync();
                    break;
                case VideoRecorderState.RecordingStopped:
                    this.buttonInit.Enabled = true;
                    this.buttonRecord.Enabled = true;
                    this.buttonStop.Enabled = false;
                    this.numericUpDownRecFramesPerSecond.Enabled = true;
                    _RecordingStopwatch.Stop();
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonInit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ButtonInit_Click(object sender, EventArgs e)
        {
            this.videoRecorder.InitRecording();
        }

        /// <summary>
        /// Handles the Click event of the buttonStopRecording control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            this.videoRecorder.StopRecording();
        }

        /// <summary>
        /// Handles the Click event of the buttonRecord control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ButtonRecord_Click(object sender, EventArgs e)
        {
            this.videoRecorder.StartRecording();
        }

        /// <summary>
        /// Handles the ValueChanged event of the numericUpDownRecFramesPerSecond control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownRecFramesPerSecond_ValueChanged(object sender, EventArgs e)
        {
            this.videoRecorder.Settings.FramesPerSecond = (int)this.numericUpDownRecFramesPerSecond.Value;
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            Size newSize = new Size((int)numericUpDownWidth.Value, videoRecorder.Settings.FrameSize.Height);
            videoRecorder.Settings.FrameSize = newSize;
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            Size newSize = new Size(videoRecorder.Settings.FrameSize.Width, (int)numericUpDownHeight.Value);
            videoRecorder.Settings.FrameSize = newSize;
        }
    }
}