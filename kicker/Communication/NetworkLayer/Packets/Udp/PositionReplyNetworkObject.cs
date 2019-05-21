namespace Communication.NetworkLayer.Packets.Udp
{
    using GlobalDataTypes;

    /// <summary>
    /// The networkobject containing the reply from the position request.
    /// </summary>
    public class PositionReplyNetworkObject : NetworkObject
    {
        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>The position.</value>
        public ushort Position { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="Bar"/>.
        /// </summary>
        public Bar Bar { get; set; }
        
        /// <summary>
        /// Gets or sets the current angel.
        /// </summary>
        public short Angel { get; set; }
    }
}