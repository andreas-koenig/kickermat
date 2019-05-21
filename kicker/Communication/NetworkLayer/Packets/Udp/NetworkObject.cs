namespace Communication.NetworkLayer.Packets.Udp
{
    using Enums;

    /// <summary>
    /// The networkobject containg the data sent or received from the network.
    /// </summary>
    public class NetworkObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkObject"/> class.
        /// </summary>
        public NetworkObject()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="UdpPacketType"/> of the packet.
        /// </summary>
        public UdpPacketType PacketType { get; set; }
    }
}