namespace Communication.NetworkLayer.Packets.Udp.Enums
{
    /// <summary>
    /// They type of the received udp packet.
    /// </summary>
    public enum UdpPacketType
    {
        /// <summary>
        /// Udp packet contains informations about player positions and angles.
        /// </summary>
        SetPositionsAndAngles = 0x00,

        /// <summary>
        /// Udp packet contains informations about the current positions.
        /// </summary>
        Position = 0x01,

        /// <summary>
        /// Udp packet contains informations about the calibration status.
        /// </summary>
        CalibrationStatus = 0x02,

        /// <summary>
        /// Udp packet contains informations about maximal possible position.
        /// </summary>
        SetMaxPosition = 0x03,

        /// <summary>
        /// Udp packet contains informations about minimal possible position.
        /// </summary>
        SetMinPosition = 0x04,

        /// <summary>
        /// Udp packet contains informations about the bar length in pixel.
        /// </summary>
        SetBarLengthInPixel = 0x05,

        /// <summary>
        /// Udp packet contains informations about the null angle.
        /// </summary>
        SetNullAngle = 0x06
    }
}