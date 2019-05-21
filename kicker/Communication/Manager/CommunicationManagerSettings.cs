namespace Communication.Manager
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Class for storing the Settings of the communication manager
    /// </summary>
    public class CommunicationManagerSettings
    {
        /// <summary>
        /// Gets or sets the communication set.
        /// </summary>
        /// <value>The communication set.</value>
        [XmlIgnore]
        public Type CommunicationSet { get; set; }

        /// <summary>
        /// Gets or sets the communication set string.
        /// </summary>
        /// <value>The communication set string.</value>
        public string CommunicationSetString
        {
            get
            {
                return this.CommunicationSet.ToString();
            }

            set
            {
                this.CommunicationSet = Type.GetType(value);
            }
        }
    }
}