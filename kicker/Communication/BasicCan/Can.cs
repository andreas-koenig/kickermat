#if CAN_ENABLED
namespace Communication.BasicCan
{
    using System;
    using Ixxat.Vci3.Bal.Can;

    /// <summary>
    /// Klasse zur Abstraktion der CAN-Funktionalität von der verwendeten CAN-Hardware
    /// </summary>
    public class Can
    {
        /// <summary>
        /// Verwendetes Basis-CAN Modul (enthält die Ansteuerung der CAN-Hardware)
        /// </summary>
        private readonly IxxatUsb2Can baseCan;

        /// <summary>
        /// Initializes a new instance of the <see cref="Can"/> class.
        /// </summary>
        public Can()
        {
            this.baseCan = IxxatUsb2Can.Instance;
            this.baseCan.CanMessageReceived += this.BaseCan_CanMessageReceived;
        }        

        /// <summary>
        /// Event, das ausgelöst wird, wenn eine neue Nachricht eintrifft
        /// </summary>
        public event EventHandler<CanMessageReceivedEventArgs> CanMessageReceived;                

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.baseCan.Initialize();
        }

        /// <summary>
        /// Sends the specified can message.
        /// </summary>
        /// <param name="canMessage">The can message.</param>
        public void Send(CanMessage canMessage)
        {
            if (this.baseCan != null)
            {
                this.baseCan.Send(canMessage);
            }
        }

        /// <summary>
        /// Sends the specified can messages.
        /// </summary>
        /// <param name="canMessages">The can messages.</param>
        public void Send(CanMessage[] canMessages)
        {
            if (this.baseCan != null)
            {
                this.baseCan.Send(canMessages);
            }
        }

        /// <summary>
        /// Handles the CanMessageReceived event of the baseCan control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanMessageReceivedEventArgs"/> instance containing the event data.</param>
        private void BaseCan_CanMessageReceived(object sender, CanMessageReceivedEventArgs e)
        {
            if (this.CanMessageReceived != null)
            {
                this.CanMessageReceived(this, e);
            }
        }
    }
}
#endif