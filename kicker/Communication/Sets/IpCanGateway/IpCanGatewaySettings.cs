namespace Communication.Sets.IpCanGateway
{
    /// <summary>
    /// Settings for the IP-CAN-Gateway
    /// </summary>
    public class IpCanGatewaySettings
    {
        /// <summary>
        /// Gets or sets the gateway address.
        /// </summary>
        /// <value>The gateway address.</value>
        public string GatewayAddress { get; set; }

        /// <summary>
        /// Gets or sets the TCP port.
        /// </summary>
        /// <value>The TCP port.</value>
        public ushort TcpPort { get; set; }

        /// <summary>
        /// Gets or sets the UDP port.
        /// </summary>
        /// <value>The UDP port.</value>
        public ushort UdpPort { get; set; }

        /// <summary>
        /// Gets or sets the position message interval.
        /// </summary>
        /// <value>The position message interval.</value>
        public int PositionMessageInterval { get; set; }
    }
}