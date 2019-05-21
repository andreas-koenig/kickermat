namespace Communication.NetworkLayer.Packets.Udp
{
    using GlobalDataTypes;

    /// <summary>
    /// The networkobject containing the reply from the calibration.
    /// </summary>
    public class CalibrationStatusReplyNetworkObject : NetworkObject
    {
        /// <summary>
        /// Gets or sets the <see cref="Bar"/> to use.
        /// </summary>
        public Bar Bar { get; set; }
    }
}