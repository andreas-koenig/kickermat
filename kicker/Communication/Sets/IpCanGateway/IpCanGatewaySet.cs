namespace Communication.Sets.IpCanGateway
{
    using System;
    using System.Net;
    using System.Windows.Forms;
    using Calibration;
    using Calibration.UdpGateway;
    using NetworkLayer.Udp;
    using PlayerControl;
    using PlayerControl.UdpGateway;
    using PluginSystem;
    using PluginSystem.Configuration;
    using Utilities;

    /// <summary>
    /// Communication set for using an IP-CAN-Gateway.
    /// </summary>
    public class IpCanGatewaySet : ICommunicationSet, IXmlConfigurableKickerPlugin
    {
        /// <summary>
        /// Gets the Settings.
        /// </summary>
        /// <value>The Settings.</value>
        public IpCanGatewaySettings Settings { get; private set; }

        /// <summary>
        /// Gets the player control.
        /// </summary>
        /// <value>The player control.</value>
        public IPlayerControl PlayerControl { get; private set; }

        /// <summary>
        /// Gets the calibration control.
        /// </summary>
        /// <value>The calibration control.</value>
        public ICalibrationControl CalibrationControl { get; private set; }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl { get; private set; }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.Settings = SettingsSerializer.LoadSettingsFromXml<IpCanGatewaySettings>(xmlFileName);
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
            this.SettingsUserControl = new IpCanGatewayUserControl(this.Settings);
            this.Connect();
        }

        /// <summary>
        /// Connects to the communication interface.
        /// </summary>
        public void Connect()
        {
            NetworkLayer networkLayer = ServiceLocator.LocateService<NetworkLayer>();
            if (networkLayer == null)
            {
                networkLayer = new NetworkLayer();
                ServiceLocator.RegisterService<NetworkLayer>(networkLayer);                
            }

            try
            {
                networkLayer.Connect(IPAddress.Parse(this.Settings.GatewayAddress), this.Settings.UdpPort, this.Settings.TcpPort);
            }
            catch (Exception ex)
            {
                SwissKnife.ShowException(this, ex);
            }

            this.PlayerControl = new UdpPlayerControl();
            this.CalibrationControl = new UdpCalibration();

            ((UdpPlayerControl)this.PlayerControl).MessageIntervall = this.Settings.PositionMessageInterval;
        }

        /// <summary>
        /// Disconnects the communication interface.
        /// </summary>
        public void Disconnect()
        {
            if (ServiceLocator.LocateService<NetworkLayer>() != null)
            {
                ServiceLocator.LocateService<NetworkLayer>().Disconnect();
            }
        }
    }
}