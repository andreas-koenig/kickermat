namespace ObjectSearch.Base
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Class which stores the basic settings of an object detection.
    /// </summary>
    public class BasicObjectSearchSettings
    {
        /// <summary>
        /// Gets or sets the image binarization algorithm.
        /// </summary>
        /// <value>The image binarization algorithm.</value>
        [XmlIgnore]
        [Browsable(false)]
        public Type ImageBinarizationAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the image binarization algorithm string.
        /// </summary>
        /// <value>The image binarization algorithm string.</value>
        [Browsable(false)]
        public string ImageBinarizationAlgorithmString
        {
            get
            {
                return this.ImageBinarizationAlgorithm.AssemblyQualifiedName;
            }

            set
            {
                this.ImageBinarizationAlgorithm = Type.GetType(value);
            }
        }
    }
}
