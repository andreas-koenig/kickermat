namespace Communication.NetworkLayer.Packets.Tcp.Enum
{
    /// <summary>
    /// The type of the received TCP packet.
    /// </summary>
    public enum TcpPacketType : ushort
    {
        /// <summary>
        /// TCP packet with logging informations.
        /// </summary>
        Logging = 0x00,

        /// <summary>
        /// TCP packet is not recognized.
        /// </summary>
        UnknownPacketType = 0xFFFF
    }
}
