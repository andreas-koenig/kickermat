#if CAN_ENABLED
namespace Communication.BasicCan
{
    using System;
    using Ixxat.Vci3.Bal.Can;

    /// <summary>
    /// Event-Args für ein MessageReceived Event
    /// </summary>
    public class CanMessageReceivedEventArgs : EventArgs
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="CanMessageReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The received message.</param>
        public CanMessageReceivedEventArgs(CanMessage message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public CanMessage Message { get; private set; }
    }
}
#endif