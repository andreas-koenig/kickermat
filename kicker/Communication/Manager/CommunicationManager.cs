namespace Communication.Manager
{
    using System;
    using System.Windows.Forms;
    using Calibration;
    using PlayerControl;
    using PluginSystem;
    using PluginSystem.Configuration;
    using Sets;

    /// <summary>
    /// Verwaltet die vorhandenen Kommunikationsschnittstellen
    /// </summary>
    public sealed class CommunicationManager : IXmlConfigurableKickerPlugin, IDisposable
    {
        /// <summary>
        /// Occurs when [communication set changed].
        /// </summary>
        public event EventHandler<NewCommunicationSetEventArgs> CommunicationSetChanged;        

        /// <summary>
        /// Gets the currently used communication set.
        /// </summary>
        public ICommunicationSet CommunicationSet { get; private set; }

        /// <summary>
        /// Gets the user control.
        /// </summary>
        /// <value>The user control.</value>
        public UserControl SettingsUserControl { get; private set; }

        /// <summary>
        /// Gets the player control.
        /// </summary>
        /// <value>The player control.</value>
        public IPlayerControl PlayerControl
        {
            get
            {
                return this.CommunicationSet.PlayerControl;
            }
        }

        /// <summary>
        /// Gets the calibration control.
        /// </summary>
        /// <value>The calibration control.</value>
        public ICalibrationControl CalibrationControl
        {
            get
            {
                return this.CommunicationSet.CalibrationControl;
            }
        }

        /// <summary>
        /// Gets or sets the Settings.
        /// </summary>
        /// <value>The Settings.</value>
        internal CommunicationManagerSettings Settings { get; set; }       

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.Settings = SettingsSerializer.LoadSettingsFromXml<CommunicationManagerSettings>(xmlFileName);
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
            this.SettingsUserControl = Plugger.CreatePluginUserControl<ICommunicationSet>(
                this,
                this.Settings.CommunicationSet,
                null,
                this.InitCommunicationSet);
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {
            if (this.SettingsUserControl != null)
            {
                this.SettingsUserControl.Dispose();
            }
        }

        /// <summary>
        /// Inits the communication set.
        /// </summary>
        /// <param name="newCommunicationSet">The new communication set.</param>
        internal void InitCommunicationSet(ICommunicationSet newCommunicationSet)
        {
            this.Settings.CommunicationSet = newCommunicationSet.GetType();
            this.CommunicationSet = newCommunicationSet;

            if (this.CommunicationSetChanged != null)
            {
                this.CommunicationSetChanged(this, new NewCommunicationSetEventArgs(newCommunicationSet));
            }
        }
    }
}