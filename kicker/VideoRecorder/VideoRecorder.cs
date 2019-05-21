namespace VideoRecorder
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using PluginSystem.Configuration;
    using Utilities;
    using System.Drawing;
    using PluginSystem;
    using VideoSource;

    /// <summary>
    /// Default implementation for a video recorder.
    /// </summary>
    public sealed class VideoRecorder : IVideoRecorder
    {
        /// <summary>
        /// The video recorder user control.
        /// </summary>
        private VideoRecorderUserControl control;

        /// <summary>
        /// Current video file name.
        /// </summary>
        private string videoFileName;

        //Current VideoWriter that saves images into a videofile.
        private VideoWriter _VideoWriter;

        private bool _Recording;


        //Reference to the currntly used VideoSource
        private IVideoSource _VideoSource;

        /// <summary>
        /// Occurs when the state state of the video recorder changed.
        /// </summary>
        public event EventHandler<VideoRecorderStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl
        {
            get
            {
                return this.control;
            }
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public VideoRecorderSettings Settings { get; set; }

        public bool Recording
        {
            get
            {
                return _Recording;
            }
        }

        /// <summary>
        /// Inits the specified instance.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        public void Init(IImage sourceImage)
        {
            if (this.StateChanged != null)
            {
                this.StateChanged(this, new VideoRecorderStateChangedEventArgs(VideoRecorderState.ImageLoaded));
            }
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.Settings = SettingsSerializer.LoadSettingsFromXml<VideoRecorderSettings>(xmlFileName);
            this.Settings.Validate();

        }

        /// <summary>
        /// Saves the configuration to a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void SaveConfiguration(string xmlFileName)
        {
            SettingsSerializer.SaveSettingsToXml(this.Settings, xmlFileName);
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public void InitUserControl()
        {
            control = new VideoRecorderUserControl(this);
        }

        /// <summary>
        /// Starts the recording.
        /// </summary>
        public void StartRecording()
        {
            if (_Recording == true)
                return;
            if (_VideoWriter != null)
            {
                _VideoWriter.Dispose();
            }
            _Recording = true;
            _VideoWriter = new VideoWriter(videoFileName, Settings.FramesPerSecond, Settings.FrameSize, true);
            _VideoWriter = new VideoWriter(videoFileName, -1, Settings.FramesPerSecond, Settings.FrameSize, true);
            _VideoSource = ServiceLocator.LocateService<IVideoSource>();
            _VideoSource.NewImage += _VideoSource_NewImage;
            if (this.StateChanged != null)
            {
                this.StateChanged(this, new VideoRecorderStateChangedEventArgs(VideoRecorderState.RecordingStarted));
            }
        }

        private void _VideoSource_NewImage(object sender, GlobalDataTypes.NewImageEventArgs e)
        {
            if (_Recording && _VideoWriter != null)
            {
                lock (this)
                {
                    Image<Bgr, Byte> bgrImage;
                    if (e.Image is Image<Bgr, Byte>)
                        bgrImage = e.Image as Image<Bgr, Byte>;
                    else
                        bgrImage = new Image<Bgr, Byte>(e.Image.Bitmap);
                    _VideoWriter.Write(bgrImage.Mat);
                }
            }
        }

        /// <summary>
        /// Stops the recording.
        /// </summary>
        public void StopRecording()
        {
            _Recording = false;
            if (_VideoSource != null)
                _VideoSource.NewImage -= _VideoSource_NewImage;
            if (_VideoWriter != null)
            {
                lock (this)
                {
                    _VideoWriter.Dispose();
                    _VideoWriter = null;
                }
            }
            if (this.StateChanged != null)
            {
                this.StateChanged(this, new VideoRecorderStateChangedEventArgs(VideoRecorderState.RecordingStopped));
            }
        }

        /// <summary>
        /// Inits the recording.
        /// </summary>
        public void InitRecording()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Video files (*.avi)|*.avi";
            sfd.InitialDirectory = this.Settings.LastDirectory;
            sfd.AddExtension = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // change enabled/disabled buttons first (because it might be overwritten by an error)
                if (this.StateChanged != null)
                {
                    this.StateChanged(this, new VideoRecorderStateChangedEventArgs(VideoRecorderState.RecordingInitialized));
                }

                FileInfo fileinfo = new FileInfo(sfd.FileName);
                this.videoFileName = this.control.VideoFileName = fileinfo.FullName;
                this.Settings.LastDirectory = fileinfo.DirectoryName;
            }
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {
            StopRecording();
            if (this.SettingsUserControl != null)
            {
                this.SettingsUserControl.Dispose();
            }
        }
    }
}
