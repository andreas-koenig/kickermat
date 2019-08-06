namespace ObjectDetection
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using ObjectSearch;
    using PluginSystem;
    using PluginSystem.Configuration;
    using Utilities;

    /// <summary>
    /// Class for basic functionality of object detection.
    /// </summary>
    /// <typeparam name="TSettings">The type of the Settings.</typeparam>
    public abstract class BasicObjectDetection<TSettings> : IBasicObjectDetection, IXmlConfigurableKickerPlugin, IDisposable
        where TSettings : BasicObjectDetectionSettings
    {
        /// <summary>
        /// Gets the thread for executing image processing.
        /// </summary>
        private readonly BackgroundWorker[] workerThreads;

        /// <summary>
        /// Counts the frames which were processed and is used to detect thread overruns 
        /// (earlier started thread finished after later started thread).
        /// </summary>
        private ulong frameCounter;

        /// <summary>
        /// The source image of the object detection.
        /// </summary>
        private IImage sourceImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicObjectDetection&lt;TSettings&gt;"/> class.
        /// </summary>
        /// <param name="numberOfThreads">The number of threads.</param>
        protected BasicObjectDetection(int numberOfThreads)
        {
            this.workerThreads = new BackgroundWorker[numberOfThreads];
            for (int workerThreadIndex = 0; workerThreadIndex < this.workerThreads.Length; workerThreadIndex++)
            {
                this.workerThreads[workerThreadIndex] = new BackgroundWorker();
                this.workerThreads[workerThreadIndex].WorkerSupportsCancellation = true;
                this.workerThreads[workerThreadIndex].DoWork += this.WorkerThread_DoWork;
            }
        }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl
        {
            get { return this.Control; }
            private set { }
        }

        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>
        /// <value>The execution count.</value>
        public int ExecutionCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                foreach (BackgroundWorker backgroundWorker in this.workerThreads)
                {
                    if (backgroundWorker.IsBusy == false)
                    {
                        return false;
                    }
                }

                // return true only if all BackgroundWorker are currently busy
                return true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IObjectDetection"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the currently used detection algorithm.
        /// </summary>
        /// <value>The detection algorithm.</value>
        public IObjectSearch ObjectSearch { get; private set; }

        /// <summary>
        /// Gets the settings of this instance.
        /// </summary>
        BasicObjectDetectionSettings IBasicObjectDetection.Settings
        {
            get
            {
                return this.Settings;
            }
        }

        /// <summary>
        /// Gets the settings in subclasses.
        /// </summary>
        protected TSettings Settings { get; private set; }

        /// <summary>
        /// Gets the user control in subclasses.
        /// </summary>
        protected BasicObjectDetectionUserControl Control { get; private set; }

        private Rectangle _PlayingFieldArea = Rectangle.Empty, _AreaOfInterestForNextSearch = Rectangle.Empty;

        public Rectangle PlayingFieldArea
        {
            get { return _PlayingFieldArea; }
            set
            {
                _PlayingFieldArea = value;
                if (this.ObjectSearch != null)
                    this.ObjectSearch.PLayingFieldArea = _PlayingFieldArea;
            }
        }

        public Rectangle AreaOfInterestForNextSearch
        {
            get { return _AreaOfInterestForNextSearch; }
            set
            {
                _AreaOfInterestForNextSearch = value;
                if (this.ObjectSearch != null)
                    this.ObjectSearch.AreaOfInterestForNextSearch = _AreaOfInterestForNextSearch;
            }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute(IImage image)
        {
            sourceImage = image;
            if (this.Settings.DetectionEnabled == true)
            {
                foreach (BackgroundWorker backgroundWorker in this.workerThreads)
                {
                    if (backgroundWorker.IsBusy == false)
                    {
                        backgroundWorker.RunWorkerAsync();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.Settings = SettingsSerializer.LoadSettingsFromXml<TSettings>(xmlFileName);
        }

        /// <summary>
        /// Saves the configuration to a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void SaveConfiguration(string xmlFileName)
        {
            SettingsSerializer.SaveSettingsToXml(this.Settings, xmlFileName);

            // TODO: wie Dispose() über PluginSystemUserControl aufrufen
            Plugger.SavePluginSettings<IObjectSearch>(this.ObjectSearch);
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public virtual void InitUserControl()
        {
            //UserControl test = Plugger.CreatePluginUserControl<IObjectSearch>(
            //    this,
            //    this.Settings.ObjectSearchAlgorithm,
            //    null,
            //    this.InitImageProcessingAlgorithm);
            //this.Control = new BasicObjectDetectionUserControl(this, test);
        }

        /// <summary>
        /// Inits the image processing algorithm.
        /// </summary>
        /// <param name="newAlgorithm">The new algorithm.</param>
        public void InitImageProcessingAlgorithm(IObjectSearch newAlgorithm)
        {
            this.Settings.ObjectSearchAlgorithm = newAlgorithm.GetType();
            this.ObjectSearch = newAlgorithm;
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.Control.Dispose();
        }

        /// <summary>
        /// Detects the objects.
        /// </summary>
        /// <param name="frameCounter">The frame counter.</param>
        protected abstract void DetectObjects(ulong frameCounter);

        /// <summary>
        /// Handles the DoWork event of the workerThread control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void WorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.Settings.DetectionEnabled == true)
            {
                var framecount = this.frameCounter++;
                this.ExecutionCount++;
                this.ObjectSearch.Execute(this.sourceImage, framecount);
                this.DetectObjects(framecount);
            }
        }
    }
}