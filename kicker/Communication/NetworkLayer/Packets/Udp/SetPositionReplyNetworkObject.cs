namespace Communication.NetworkLayer.Packets.Udp
{
    using GlobalDataTypes;

    /// <summary>
    /// The networkobject containing the reply from the set position request.
    /// </summary>
    public class SetPositionReplyNetworkObject : NetworkObject
    {
        /// <summary>
        /// Gets or sets the <see cref="Bar"/>.
        /// </summary>
        public Bar Bar { get; set; }

        /// <summary>
        /// Gets or sets he current position.
        /// </summary>
        public ushort Position { get; set; }
    }
}