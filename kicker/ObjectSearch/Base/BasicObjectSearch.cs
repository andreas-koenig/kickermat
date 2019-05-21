namespace ObjectSearch.Base
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using GlobalDataTypes;
    using ImageProcessing;
    using ObjectSearchPreparation;
    using PluginSystem;
    using PluginSystem.Configuration;

    /// <summary>
    /// Class which implements basic funtionality for detecting objects in a RGB image.
    /// The RGB image will be processed with <see cref="IImageProcessor"/> before executing object search.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    public abstract class BasicObjectSearch<TSettings> : IObjectSearch
        where TSettings : BasicObjectSearchSettings
    {
        /// <summary>
        /// The used algorithm for image binarization.
        /// </summary>
        private IObjectSearchPreparation imageBinarization;
        /// <summary>
        /// Reference to the original image in RGB format.
        /// </summary>
        public IImage currentSourceImage;

        /// <summary>
        /// Gets or sets a callback when a new binary image is available.
        /// </summary>
        public NewBinaryImageCallback NewBinaryImageCallback { get; set; }

        /// <summary>
        /// Gets or sets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl { get; protected set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public TSettings Settings { get; private set; }

        /// <summary>
        /// Gets the number of found objects.
        /// </summary>
        /// <value>The number of found objects.</value>
        public abstract int NumberOfFoundObjects { get; }

        /// <summary>
        /// Gets the child user control.
        /// </summary>
        /// <value>The child user control.</value>
        protected abstract UserControl ChildUserControl { get; }

        public Rectangle PLayingFieldArea
        {
            get; set;
        } = Rectangle.Empty;

        public Rectangle AreaOfInterestForNextSearch
        {
            get; set;
        } = Rectangle.Empty;

        /// <summary>
        /// Executes a conversion and blob detection run.
        /// </summary>
        public void Execute(IImage image, ulong framecount)
        {
            this.currentSourceImage = image;
            if (this.currentSourceImage == null)
                return;
            var resultImage = this.imageBinarization.Execute(this.currentSourceImage);
            this.FindObjects(resultImage,framecount);
            //Reset ROI of Image for correct displaying in userControls if used.
            if (resultImage is Image<Gray, byte>)
                (resultImage as Image<Gray, byte>).ROI = Rectangle.Empty;
            if (this.NewBinaryImageCallback != null)
            {
                this.NewBinaryImageCallback(resultImage);
            }
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public void InitUserControl()
        {
            PluginSystemUserControl<IObjectSearchPreparation> imageBinarizationUserControl = new PluginSystemUserControl<IObjectSearchPreparation>(
                this,
                this.Settings.ImageBinarizationAlgorithm,
                this.UpdateObjectSearchPreparation);
            BasicObjectSearchUserControl userControl = new BasicObjectSearchUserControl(imageBinarizationUserControl, this.ChildUserControl);

            this.SettingsUserControl = userControl;
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public virtual void LoadConfiguration(string xmlFileName)
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
            Plugger.SavePluginSettings<IObjectSearchPreparation>(this.imageBinarization);
        }

        /// <summary>
        /// Retunrs the size of a found object.
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <returns>The size of the object.</returns>
        public abstract int ObjectSize(int index);

        /// <summary>
        /// Sorts the objects with the current sort settings.
        /// </summary>
        public abstract void SortObjects();

        /// <summary>
        /// Gets the object center.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The object center.</returns>
        public abstract Position GetObjectCenter(int index);

        /// <summary>
        /// Gets the bounding box of an object.
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <returns>The bounding box of the object.</returns>
        public abstract Rectangle GetObjectBounds(int index);

        /// <summary>
        /// Finds the objects in the binarized image.
        /// </summary>
        /// <param name="binarizedImage">The binarized image.</param>
        protected abstract void FindObjects(IImage binarizedImage, ulong framecount);

        /// <summary>
        /// Updates the image binarization algorithm.
        /// </summary>
        /// <param name="newAlgorithm">The new algorithm.</param>
        private void UpdateObjectSearchPreparation(IObjectSearchPreparation newAlgorithm)
        {
            this.imageBinarization = newAlgorithm;
            this.Settings.ImageBinarizationAlgorithm = newAlgorithm.GetType();
        }

        public abstract int GetBiggestObjectIndex();
    }
}