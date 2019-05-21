namespace Communication.Sets.IpCanGateway
{
    using System.Windows.Forms;

    /// <summary>
    /// User control for setting up the connection parameters for the IP-CAN-Gateway.
    /// </summary>
    public partial class IpCanGatewayUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IpCanGatewayUserControl"/> class.
        /// </summary>
        /// <param name="settings">The Settings.</param>
        public IpCanGatewayUserControl(IpCanGatewaySettings settings)
        {
            this.InitializeComponent();
            this.propertyGridSettings.SelectedObject = settings;
        }
    }
}