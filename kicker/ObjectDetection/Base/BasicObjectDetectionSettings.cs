namespace ObjectDetection
{
    using System;
    using System.Xml.Serialization;
    using ObjectSearch.BlobSearch;

    /// <summary>
    /// Settings for basic object detection.
    /// </summary>
    public abstract class BasicObjectDetectionSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether this object detection is eanbaled.
        /// </summary>
        public bool DetectionEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the display of the user control gets updated.
        /// </summary>
        public bool UpdateDisplay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether labels for the detected objects are shown.
        /// </summary>
        public bool ShowLabels { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the object labels are also shown in the main display.
        /// </summary>
        public bool LabelsOnMain { get; set; }

        /// <summary>
        /// Gets or sets the image processing algorithm.
        /// </summary>
        /// <value>The image processing algorithm.</value>
        [XmlIgnore]
        public Type ObjectSearchAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the image processing algorithm string.
        /// </summary>
        /// <value>The image processing algorithm string.</value>
        public string ObjectSearchAlgorithmString
        {
            get
            {
                return this.ObjectSearchAlgorithm.AssemblyQualifiedName;
            }

            set
            {
                this.ObjectSearchAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            if (this.ObjectSearchAlgorithm == null)
            {
                this.ObjectSearchAlgorithm = typeof(BlobSearch);
            }
        }
    }
}