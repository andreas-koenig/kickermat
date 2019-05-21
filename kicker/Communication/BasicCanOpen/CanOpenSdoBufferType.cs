namespace Communication.BasicCanOpen
{
    using System.Threading;

    /// <summary>
    /// Structure for the read buffer of CANopen SDO messages. 
    /// Read-Request which are sent to a node and the corresponding responses are stored with this structure
    /// </summary>
    public class SdoBufferType
    {
        /// <summary>
        /// Gets or sets the ID of the receiving node
        /// </summary>
        /// <value>The node ID.</value>
        public byte NodeId { get; set; }

        /// <summary>
        /// Gets or sets the SDO index of the read object
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the SDO subindex of the read object
        /// </summary>
        public byte SubIndex { get; set; }

        /// <summary>
        /// Gets or sets the read value
        /// </summary>
        public int ReceivedValue { get; set; }

        /// <summary>
        /// Gets or sets the event which is used for awaiting the response
        /// </summary>
        public AutoResetEvent EventSemaphore { get; set; }
    }
}