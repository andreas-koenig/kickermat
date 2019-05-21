namespace Communication.BasicCanOpen
{
    using System.Threading;

    /// <summary>
    /// Structure for the read buffer of CANopen NMT messages. 
    /// Read-Request which are sent to a node and the corresponding responses are stored with this structure
    /// </summary>
    public class NmtReadBufferType
    {
        /// <summary>
        /// Gets or sets the ID of the receiving node
        /// </summary>
        public byte NodeId { get; set; }

        /// <summary>
        /// Gets or sets the read value
        /// </summary>
        public byte ReceivedValue { get; set; }

        /// <summary>
        /// Gets or sets the event which is used for awaiting the response
        /// </summary>
        public AutoResetEvent EventSemaphore { get; set; }
    }
}