#if CAN_ENABLED
namespace Communication.PlayerControl.CanCanGateway
{
    using System;
    using System.Threading;
    using BasicCan;
    using GlobalDataTypes;
    using Ixxat.Vci3.Bal.Can;

    /// <summary>
    /// Class for controlling players with a CAN-CAN-Gateway
    /// </summary>
    public class CanCanGatewayPlayerControl : Can, IPlayerControl, IDisposable
    {
        /// <summary>
        /// Event which is used for awaiting the response for a move bar request
        /// </summary>
        private readonly AutoResetEvent resetForMoveBarRequest;

        /// <summary>
        /// Event which is used for awaiting the response for a rotate bar request
        /// </summary>
        private readonly AutoResetEvent resetForRotateBarRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanCanGatewayPlayerControl"/> class.
        /// </summary>
        public CanCanGatewayPlayerControl()
        {
            this.resetForMoveBarRequest = new AutoResetEvent(false);
            this.resetForRotateBarRequest = new AutoResetEvent(false);

            CanMessageReceived += this.CanPlayerControl_CanMessageReceived;
        }       

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }                

        /// <summary>
        /// Positioniert eine Stange
        /// </summary>
        /// <param name="playerBar">Die zu positionierende Stange</param>
        /// <param name="newPlayerPosition">Die gewünschte Position</param>
        public void MovePlayer(Bar playerBar, ushort newPlayerPosition)
        {
            this.MovePlayer(playerBar, newPlayerPosition, false);
        }

        /// <summary>
        /// Bewegt die gewünschte Stange reltaiv zum Nullpunkt um die übergebene Anzahl an Pixel
        /// </summary>
        /// <param name="playerBar">Die zu drehende Stange</param>
        /// <param name="newPlayerPosition">Die Anzahl der Pixel, um die die Stange (relativ zum Nullpunkt) bewegt werden soll</param>
        /// <param name="waitForResponse">Legt fest, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        public void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse)
        {
            CanMessage canMessage = new CanMessage
                                        {
                                            TimeStamp = 0,
                                            Identifier = (uint)MessageId.MoveBarRequestBaseId + (uint)playerBar,
                                            FrameType = CanMsgFrameType.Data,
                                            DataLength = 8
                                        };
            canMessage[0] = (byte)(newPlayerPosition >> 8);
            canMessage[1] = (byte)newPlayerPosition;

            if (waitForResponse)
            {
                canMessage[2] = 0x01;
            }
            else
            {
                canMessage[2] = 0x00;
            }

            canMessage[3] = 0xFF;
            canMessage[4] = 0xFF;
            canMessage[5] = 0xFF;
            canMessage[6] = 0xFF;
            canMessage[7] = 0xFF;

            Send(canMessage);

            if (waitForResponse)
            {
                this.resetForMoveBarRequest.WaitOne(new TimeSpan(0, 0, 5));
            }
            else
            {
                Thread.Sleep(7);
            }
        }

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        public void SetAngle(Bar bar, short angle)
        {
            this.SetAngle(bar, angle, false);
        }

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        public void SetAngle(Bar bar, short angle, bool waitForResponse)
        {
            CanMessage canMsg = new CanMessage
                                    {
                                        TimeStamp = 0,
                                        Identifier = (uint)MessageId.RotateBarRequestBaseId + (uint)bar,
                                        FrameType = CanMsgFrameType.Data,
                                        DataLength = 8
                                    };
            canMsg[0] = (byte)(angle >> 8);
            canMsg[1] = (byte)angle;
            if (waitForResponse)
            {
                canMsg[2] = 0x01;
            }
            else
            {
                canMsg[2] = 0x00;
            }

            canMsg[3] = 0xFF;
            canMsg[4] = 0xFF;
            canMsg[5] = 0xFF;
            canMsg[6] = 0xFF;
            canMsg[7] = 0xFF;

            // Write the CAN message into the transmit FIFO
            Send(canMsg);

            if (waitForResponse)
            {
                this.resetForRotateBarRequest.WaitOne(new TimeSpan(0, 0, 5));
            }
            else
            {
                Thread.Sleep(7);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.resetForMoveBarRequest.Close();
                this.resetForRotateBarRequest.Close();
            }
        }

        /// <summary>
        /// Handles the CanMessageReceived event of the CanPlayerControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanMessageReceivedEventArgs"/> instance containing the event data.</param>
        private void CanPlayerControl_CanMessageReceived(object sender, CanMessageReceivedEventArgs e)
        {
            switch (e.Message.Identifier & 0xFFF0)
            {
                case (int)MessageId.MoveBarResponseBaseId:
                    this.resetForMoveBarRequest.Set();
                    break;
                case (int)MessageId.RotateBarResponseBaseId:
                    this.resetForRotateBarRequest.Set();
                    break;
            }
        }
    }
}
#endif