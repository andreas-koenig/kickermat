namespace Kicker
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using Communication.Manager;
    using DataProcessing;
    using Game;
    using GlobalDataTypes;
    using Logging;
    using Logging.log4net.Appenders;
    using PluginSystem;
    using PluginSystem.Configuration;
    using Version;
    using VideoRecorder;
    using VideoSource;
    using VideoSource.AviFile;
    using VideoSource.ProsilicaGE680_Vimba;
    using System.Drawing;
    using VideoSource.StaticImageVideoSource;

    /// <summary>
    /// The main form of the application.
    /// </summary>
    public sealed partial class FormMain : Form
    {
        /// <summary>
        /// Instanz zur Kontrolle der Kameraeinstellungen und Abfrage der Attribute der Eingangsquelle
        /// Wird nach jedem Laden einer Videoquelle neu initialisiert,
        /// aber nur einmal instanziert, damit man nicht jedes Mal das UserControl ändern muss.
        /// </summary>
        private IVideoSource videoSource;

        /// <summary>
        /// Instanz zur Verarbeitung der Bilddaten
        /// Wird nach jedem Laden einer Videoquelle neu initialisiert,
        /// aber nur einmal instanziert, damit man nicht jedes Mal das UserControl ändern muss.
        /// </summary>
        private DataProcessor dataProcessor;

        /// <summary>
        /// Instance for recording videos.
        /// </summary>
        private IVideoRecorder videoRecorder;

        /// <summary>
        /// Instance for communication Settings.
        /// </summary>
        private CommunicationManager communicationManger;

        /// <summary>
        /// Plugin independent settings.
        /// </summary>
        private KickerMainSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// </summary>
        public FormMain()
        {
            // Initialize log4net first to get all logs from the following initializations
            log4net.Config.XmlConfigurator.Configure();

            this.InitializeComponent();
            this.InitLogging();
            this.LoadMainSettings();

            // Communication manager must be started first because it's neccessary for following modules.
            this.InitCommunicationManager();
            this.InitDataProcessing();
            this.InitVideoRecorder();
            this.InitVideoSource<ProsilicaGE680CVideoSource_Vimba>();
        }

        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="image">The image.</param>
        private static void SaveImage(Bitmap image)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Windows Bitmap (*.bmp)|*.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                image.Save(sfd.FileName);
            }
        }

        /// <summary>
        /// Inits the communication manager.
        /// </summary>
        private void InitCommunicationManager()
        {
            this.communicationManger = Plugger.CreatePluginInstance<CommunicationManager>(this, typeof(CommunicationManager));
            ServiceLocator.RegisterService(this.communicationManger);
            this.communicationManger.SettingsUserControl.Dock = DockStyle.Fill;
            this.tabPageCommunication.Controls.Add(this.communicationManger.SettingsUserControl);
        }

        /// <summary>
        /// Loads the settings and initializes the form with them.
        /// </summary>
        private void LoadMainSettings()
        {
            string xmlFileName = AppSettings.ConfigFilesPath;
            xmlFileName += Path.DirectorySeparatorChar + "KickerMain.xml";
            this.settings = SettingsSerializer.LoadSettingsFromXml<KickerMainSettings>(xmlFileName);
            this.settings.Validate();

            this.Left = this.settings.MainWindowLeft;
            this.Top = this.settings.MainWindowTop;

            //FormImageDisplay.Instance.Left = this.settings.ImageDisplayLeft;
            //FormImageDisplay.Instance.Top = this.settings.ImageDisplayTop;

            if (this.settings.LastVideoOrDriver.Equals(string.Empty))
            {
                this.toolStripOpenLast.Enabled = false;
                this.toolStripOpenLast.ToolTipText = "Open last Driver or Video again";
            }
            else
            {
                this.toolStripOpenLast.Enabled = true;
                this.toolStripOpenLast.ToolTipText = "Open last Driver or Video again:\n" + this.settings.LastVideoOrDriver;
            }

            if (this.settings.SelectedTab >= 0 &&
                this.settings.SelectedTab < this.tabControlMainForm.TabCount)
            {
                this.tabControlMainForm.SelectedIndex = this.settings.SelectedTab;
            }

            this.Text = this.Text + " " + KickerVersion.Version;
        }

        /// <summary>
        /// Collects and saves the settings of the main form.
        /// </summary>
        private void SaveMainSettings()
        {
            this.settings.MainWindowTop = this.Top;
            this.settings.MainWindowLeft = this.Left;
            this.settings.SelectedTab = this.tabControlMainForm.SelectedIndex;

            string xmlFileName = AppSettings.ConfigFilesPath;
            xmlFileName += Path.DirectorySeparatorChar + "KickerMain.xml";
            SettingsSerializer.SaveSettingsToXml(this.settings, xmlFileName);
        }

        /// <summary>
        /// Force all modules to save their settings.
        /// </summary>
        private void SaveModuleSettings()
        {
            // TODO: wie Dispose() über PluginSystemUserControl aufrufen
            Plugger.SavePluginSettings(this.dataProcessor);
            Plugger.SavePluginSettings(this.communicationManger);
            Plugger.SavePluginSettings(this.videoRecorder);
            Plugger.SavePluginSettings(this.videoSource);
        }

        /// <summary>
        /// Inits the logging.
        /// </summary>
        private void InitLogging()
        {
            // Load the serial log reader for receiving the gateway messages
            SerialLogReader logReader = new SerialLogReader();
            logReader.Dock = DockStyle.Fill;
            this.tabPageGatewayMessages.Controls.Add(logReader);

            // Load the list box appender to display log4net messages
            ListBoxAppender listBoxAppender = ListBoxAppender.GetAppender(this.GetType());
            if (listBoxAppender != null)
            {
                listBoxAppender.SettingsUserControl.Dock = DockStyle.Fill;
                this.tabPageLogging.Controls.Add(listBoxAppender.SettingsUserControl);
            }
        }

        /// <summary>
        /// Inits the video recorder.
        /// </summary>
        private void InitVideoRecorder()
        {
            this.videoRecorder = Plugger.CreatePluginInstance<IVideoRecorder>(this, typeof(VideoRecorder));
            this.tabPageVideoRecorder.Controls.Add(this.videoRecorder.SettingsUserControl);
            if (this.videoRecorder.SettingsUserControl != null)
                this.videoRecorder.SettingsUserControl.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Inits the video source.
        /// </summary>
        /// <typeparam name="TVideoSource">The type of the video source.</typeparam>
        private void InitVideoSource<TVideoSource>() where TVideoSource : IVideoSource, new()
        {
            if (this.videoSource != null)
            {
                this.videoSource.StopAcquisition();
                if (this.videoSource.GetType() != typeof(TVideoSource))
                {
                    this.tabPageVideoSource.Controls.Clear();
                    this.videoSource.NewImage -= this.VideoInput_NewImage;
                    Plugger.DestroyInstance(this.videoSource);
                    this.videoSource = null;
                }
            }

            if (this.videoSource == null)
            {
                this.videoSource = Plugger.CreatePluginInstance<TVideoSource>(this, typeof(TVideoSource));
                ServiceLocator.RegisterService(this.videoSource);
                this.videoSource.SettingsUserControl.Dock = DockStyle.Fill;
                this.tabPageVideoSource.Controls.Add(this.videoSource.SettingsUserControl);
                this.videoSource.NewImage += this.VideoInput_NewImage;
            }

            if (this.videoSource.GetType() == typeof(ProsilicaGE680CVideoSource_Vimba))
                this.statusLabelLoadedFile.Text = "Camera selected : ProsilicaGE680CVideoSource_Vimba";
            else if (this.videoSource.GetType() == typeof(StaticImageVideoSource))
                this.statusLabelLoadedFile.Text = "Static Image : ";
            else if (this.videoSource.GetType() == typeof(AviFileVideoSource))
                this.statusLabelLoadedFile.Text = "Loaded Avi File : ";

            // Update user controls
            Application.DoEvents();

            // Bildverarbeitung Initialisieren
            this.dataProcessor.Init(this.videoSource.RgbImage);

            // Init video recorder
            this.videoRecorder.Init(this.videoSource.RawImage);

            // Buttons aktivieren
            this.toolStripPlay.Enabled = true;
            this.toolStripSnap.Enabled = true;
            this.toolStripSave.Enabled = true;
            this.toolStripSaveRgb.Enabled = true;
            this.toolStripSaveRaw.Enabled = true;
            this.toolStripRecalibrate.Enabled = true;
            this.videoSource.StartAcquisition();
            setVideoSourceButtonsEnabled();
        }

        /// <summary>
        /// Handles the NewImage event of the VideoInput control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NewImageEventArgs"/> instance containing the event data.</param>
        private void VideoInput_NewImage(object sender, NewImageEventArgs e)
        {
            if (dataProcessor != null)
                this.dataProcessor.Execute(e.Image);
        }

        /// <summary>
        /// Inits the data processing.
        /// </summary>
        private void InitDataProcessing()
        {
            this.dataProcessor = Plugger.CreatePluginInstance<DataProcessor>(this, typeof(DataProcessor));
            this.dataProcessor.SettingsUserControl.Dock = DockStyle.Fill;
            this.tabPageDataProcessing.Controls.Add(this.dataProcessor.SettingsUserControl);
        }

        /// <summary>
        /// Shows the load video open file dialog.
        /// </summary>
        private void ShowLoadVideoOpenFileDialog()
        {
            string dir = this.settings.LastVideoDirectory;
            this.LoadNewVideoSource("Open Video File", "Video files (*.avi)|*.avi|All files (*.*)|*.*", ref dir);
            this.settings.LastVideoDirectory = dir;
        }

        /// <summary>
        /// Loads a new video source from the specified file. 
        /// </summary>
        /// <param name="fileinfo">Info about the file to load.</param>        
        private void LoadNewVideoSource(FileInfo fileinfo)
        {
            // Save the last loaded driver or video
            this.settings.LastVideoOrDriver = fileinfo.FullName;
            this.toolStripOpenLast.Enabled = true;
            this.toolStripOpenLast.ToolTipText = "Open last Driver or Video again:\n" + this.settings.LastVideoOrDriver;

            // Falls eine Bildverarbeitungskalibrierung  oder ein Spiel läuft, 
            // wird sie/es abgebrochen und gewartet bis der Thread beendet ist
            Game.StopGame();

            if (fileinfo.Name.EndsWith(".avi", true, CultureInfo.CurrentCulture))
            {
                this.InitVideoSource<AviFileVideoSource>();
            }
            else
            {
                this.InitVideoSource<ProsilicaGE680CVideoSource_Vimba>();
            }

            if (this.videoSource.LoadVideoSource(fileinfo.FullName) == true)
            {
                if (this.videoSource.GetType() == typeof(ProsilicaGE680CVideoSource_Vimba))
                    this.statusLabelLoadedFile.Text = "Camera selected : ProsilicaGE680CVideoSource_Vimba";
                else if (this.videoSource.GetType() == typeof(StaticImageVideoSource))
                    this.statusLabelLoadedFile.Text = "Static Image : " + fileinfo.FullName;
                else if (this.videoSource.GetType() == typeof(AviFileVideoSource))
                    this.statusLabelLoadedFile.Text = "Loaded Avi File : " + fileinfo.FullName;
            }
            else
            {
                this.statusLabelLoadedFile.Text = "Failed to load Driver or Video file";
                this.toolStripPlay.Enabled = false;
                this.toolStripSnap.Enabled = false;
                this.toolStripSave.Enabled = false;
                this.toolStripSaveRgb.Enabled = false;
                this.toolStripSaveRaw.Enabled = false;
                this.toolStripRecalibrate.Enabled = false;
            }
        }

        /// <summary>
        /// Loads a new video source by showing a file dialog.
        /// </summary>
        /// <param name="fileDialogTitle">The file dialog title.</param>
        /// <param name="fileDialogFilter">The file dialog filter.</param>
        /// <param name="fileDialogDirectory">The file dialog directory.</param>
        private void LoadNewVideoSource(string fileDialogTitle, string fileDialogFilter, ref string fileDialogDirectory)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = fileDialogTitle;
            ofd.Filter = fileDialogFilter;
            ofd.InitialDirectory = fileDialogDirectory;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileinfo = new FileInfo(ofd.FileName);
                fileDialogDirectory = fileinfo.DirectoryName;
                this.LoadNewVideoSource(fileinfo);
            }
        }

        /// <summary>
        /// Load the last driver or video from the saved settings.
        /// </summary>
        private void LoadLastDriverOrVideo()
        {
            if ((string.IsNullOrEmpty(this.settings.LastVideoOrDriver) == false) &&
                File.Exists(this.settings.LastVideoOrDriver))
            {
                this.LoadNewVideoSource(new FileInfo(this.settings.LastVideoOrDriver));
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the FormMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopGame();
            this.SaveMainSettings();
            if (this.videoSource != null)
            {
                this.videoSource.StopAcquisition();
                Plugger.DestroyInstance(this.videoSource);
                this.videoSource = null;
            }
            if (this.videoRecorder != null)
            {
                Plugger.DestroyInstance(this.videoRecorder);
                this.videoRecorder = null;
            }
            if (this.dataProcessor != null)
            {
                this.dataProcessor.Stop();
                Plugger.DestroyInstance(this.dataProcessor);
                this.dataProcessor = null;
            }
            if (this.communicationManger != null)
            {
                Plugger.DestroyInstance(this.communicationManger);
                this.communicationManger = null;
            }

            this.Dispose(true);
        }

        /// <summary>
        /// Stops the game and enables/disables buttons in the toolbar.
        /// </summary>
        private void StopGame()
        {
            Game.StopGame();
            this.toolStripPlay.Enabled = true;
            this.toolStripSnap.Enabled = true;
            this.toolStripStop.Enabled = false;
            setVideoSourceButtonsEnabled();
        }

        /// <summary>
        /// Starts the game and enables/disables buttons in the toolbar.
        /// </summary>
        private void StartGame()
        {
            this.toolStripPlay.Enabled = false;
            this.toolStripSnap.Enabled = false;
            this.toolStripStop.Enabled = true;
            this.toolStripStopAquisition.Enabled = false;
            this.toolStripStartAquisition.Enabled = false;
            Game.StartGame();
        }

        /// <summary>
        /// Saves an the currently displayed image.
        /// </summary>
        /// <param name="rgb">If <c>true</c> an RGB image is saved, otherwise a RAW image is saved.</param>
        private void SaveImage(bool rgb)
        {
            if (this.videoSource != null)
            {
                try
                {
                    if (rgb)
                    {
                        SaveImage(this.videoSource.RgbImage.Bitmap);
                    }
                    else
                    {
                        SaveImage(this.videoSource.RawImage.Bitmap);
                    }
                }
#pragma warning disable 168
                catch (NullReferenceException e)
#pragma warning restore 168
                {
                    // Do nothing, just catch it
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the toolStripPlay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripPlay_Click(object sender, EventArgs e)
        {
            this.StartGame();
        }

        /// <summary>
        /// Handles the Click event of the toolStripStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripStop_Click(object sender, EventArgs e)
        {
            this.StopGame();
        }

        /// <summary>
        /// Handles the Click event of the toolStripOpenVideo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripOpenVideo_Click(object sender, EventArgs e)
        {
            this.ShowLoadVideoOpenFileDialog();
        }

        /// <summary>
        /// Handles the Click event of the toolStripSaveRaw control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripSaveRaw_Click(object sender, EventArgs e)
        {
            this.SaveImage(false);
        }

        /// <summary>
        /// Handles the Click event of the toolStripSaveRgb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripSaveRgb_Click(object sender, EventArgs e)
        {
            this.SaveImage(true);
        }

        /// <summary>
        /// Handles the Click event of the toolStripSnap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripSnap_Click(object sender, EventArgs e)
        {
            if (this.videoSource != null)
            {
                this.videoSource.GetNewImage();
            }
        }

        /// <summary>
        /// Handles the ResizeBegin event of the FormMain control.
        /// It suspends the layout if resizing begins to avoid continous redrawing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FormMain_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        /// <summary>
        /// Handles the ResizeEnd event of the FormMain control.
        /// It resumes the layouting after resizing which was stopped with <see cref="FormMain_ResizeBegin"/> before.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout(true);
        }

        /// <summary>
        /// Handles the Click event of the toolStripOpenLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripOpenLast_Click(object sender, EventArgs e)
        {
            this.LoadLastDriverOrVideo();
        }

        /// <summary>
        /// Handles the Click event of the toolStripRecalibrate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripRecalibrate_Click(object sender, EventArgs e)
        {
            this.dataProcessor.ExecuteCalibration();
        }

        /// <summary>
        /// Handles the Click event of the ToolStripSaveSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripSaveSettings_Click(object sender, EventArgs e)
        {
            this.SaveMainSettings();
            this.SaveModuleSettings();
        }

        private void prosilicaVimbaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeProsilica_VimbaVideoSource();
        }

        private void InitializeProsilica_VimbaVideoSource()
        {
            this.InitVideoSource<ProsilicaGE680CVideoSource_Vimba>();
        }

        private void InitializeStaticImageVideoSource()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Windows Bitmap (*.bmp)|*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.EndsWith(".bmp"))
            {
                this.InitVideoSource<StaticImageVideoSource>();
                this.statusLabelLoadedFile.Text += "(" + ofd.FileName + ")";
                this.videoSource.LoadVideoSource(ofd.FileName);
                this.videoSource.StartAcquisition();
            }
        }

        private void staticImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeStaticImageVideoSource();
        }

        private void toolStripStartAquisition_Click(object sender, EventArgs e)
        {
            if (this.videoSource != null)
                videoSource.StartAcquisition();
            setVideoSourceButtonsEnabled();
        }

        private void setVideoSourceButtonsEnabled()
        {
            if (videoSource != null)
            {
                this.toolStripStartAquisition.Enabled = !videoSource.Acquiring;
                this.toolStripStopAquisition.Enabled = videoSource.Acquiring;
            }
            else
            {
                this.toolStripStartAquisition.Enabled = false;
                this.toolStripStopAquisition.Enabled = false;
            }
        }

        private void toolStripStopAquisition_Click(object sender, EventArgs e)
        {
            if (this.videoSource != null)
                videoSource.StopAcquisition();
            setVideoSourceButtonsEnabled();
        }
    }
}
