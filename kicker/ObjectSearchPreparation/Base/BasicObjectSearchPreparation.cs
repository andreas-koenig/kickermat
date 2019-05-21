namespace ObjectSearchPreparation.Base
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using ImageProcessing;
    using ObjectSearchPreparation;
    using PluginSystem.Configuration;
    using System.Drawing;

    /// <summary>
    /// Base class for image binarization.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    public abstract class BasicObjectSearchPreparation<TSettings> : IObjectSearchPreparation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicObjectSearchPreparation{TSettings}"/> class.
        /// </summary>
        protected BasicObjectSearchPreparation()
        {
            this.ImageProcessors = new List<IImageProcessor>();
        }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl { get; private set; }

        /// <summary>
        /// Gets a list of image processors which will be executed before starting blob detection.
        /// </summary>
        /// <value>The image processors.</value>
        protected List<IImageProcessor> ImageProcessors { get; private set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        protected TSettings Settings { get; private set; }

        /// <summary>
        /// Executes the image binarization with specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <returns>The resulting binary image.</returns>
        public IImage Execute(IImage sourceImage)
        {
            IImage[] resultImages = new IImage[this.ImageProcessors.Count];

            for (int executionIndex = 0; executionIndex < this.ImageProcessors.Count; executionIndex++)
            {
                // First call gets the source image, all following image processors get the image from the previous processor
                if (executionIndex == 0)
                {
                    resultImages[executionIndex] = this.ImageProcessors[executionIndex].Execute(sourceImage);
                }
                else
                {
                    resultImages[executionIndex] =
                        this.ImageProcessors[executionIndex].Execute(resultImages[executionIndex - 1]);
                }
            }

            return resultImages[this.ImageProcessors.Count - 1];
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.Settings = SettingsSerializer.LoadSettingsFromXml<TSettings>(xmlFileName);
            this.InitImageProcessorList();
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
            if (this.ImageProcessors.Count > 0)
            {
                BasicObjectSearchPreparationUserControl userControl = new BasicObjectSearchPreparationUserControl();

                // Reverse list to get expected order in the user interface
                this.ImageProcessors.Reverse();
                foreach (IImageProcessor imageProcessor in this.ImageProcessors)
                {
                    if (imageProcessor.SettingsUserControl != null)
                    {
                        userControl.AddNewControl(imageProcessor.SettingsUserControl);
                    }
                }

                // Undo previous reverse
                this.ImageProcessors.Reverse();

                this.SettingsUserControl = userControl;
            }
            else
            {
                this.SettingsUserControl = new UserControl();
            }
        }

        /// <summary>
        /// Inits the image processor list.
        /// </summary>
        protected abstract void InitImageProcessorList();
    }
}
