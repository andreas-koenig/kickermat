#if CAN_ENABLED
namespace Communication.Calibration.CanCanGateway
{
    using System;
    using System.Threading;
    using BasicCan;
    using GlobalDataTypes;
    using Ixxat.Vci3.Bal.Can;
    using NetworkLayer.Packets.Udp.Enums;
    using PlayerControl.CanCanGateway;

    /// <summary>
    /// Auflistung der Bewegungsbefehle, die für alle Stangen gleichzeitig ausgeführt werden können
    /// </summary>
    public enum PositionRequest
    {
        /// <summary>
        /// Alle Stangen auf minimale Position bewegen
        /// </summary>
        MoveAllBarsToMinimumPosition = 0x00,
        
        /// <summary>
        /// Alle Stangen auf maximale Position bewegen
        /// </summary>
        MoveAllBarsToMaximumPosition = 0xFF
    }

    /// <summary>
    /// Stellt alle Funktionen zur verfügung, die zur Initialisierung und Kalibrierung der Bildverarbeitung benötigt werden
    /// </summary>
    public class CanCanGatewayCalibration : CanCanGatewayPlayerControl, ICalibrationControl
    {
        /// <summary>
        /// Wird verwendet, um auf die Antwort einer Init-Status Anfrage zu warten
        /// </summary>
        private readonly AutoResetEvent eventForInitStatusRequest = new AutoResetEvent(false);

        /// <summary>
        /// Wird verwendet, um auf die Antwort einer Max-Position Anfrage zu warten
        /// </summary>
        private readonly AutoResetEvent eventForMaxPositionRequest = new AutoResetEvent(false);
        
        /// <summary>
        /// Wird verwendet, um auf die Antwort einer Min-Position Anfrage zu warten
        /// </summary>
        private readonly AutoResetEvent eventForMinPositionRequest = new AutoResetEvent(false);

        /// <summary>
        /// Backend-Variable für <see cref="InitStatus"/>
        /// </summary>
        private ControllerStatus currentInitStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanCanGatewayCalibration"/> class.
        /// </summary>
        public CanCanGatewayCalibration()
        {
            this.Initialize();
            this.CanMessageReceived += this.CanAbstractionForCalibrationCanMessageReceived;
        }       

        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        public ControllerStatus InitStatus
        {
            get
            {
                CanMessage message = new CanMessage();
                message.TimeStamp = 0;
                message.Identifier = (uint)MessageId.InitStatusRequestBaseId;
                message.FrameType = CanMsgFrameType.Data;
                message.DataLength = 8;
                message[0] = 0xFF;
                message[1] = 0xFF;
                message[2] = 0xFF;
                message[3] = 0xFF;
                message[4] = 0xFF;
                message[5] = 0xFF;
                message[6] = 0xFF;
                message[7] = 0xFF;
                Send(message);
                if (this.eventForInitStatusRequest.WaitOne(10))
                {
                    return this.currentInitStatus;
                }

                return ControllerStatus.Error;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Moves all bars to their minimum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public ReturnType MoveAllBarsToMinimumPosition()
        {
            ReturnType operationSuccessful = ReturnType.Ok;
            foreach (Bar barName in Enum.GetValues(typeof(Bar)))
            {
                CanMessage message = new CanMessage();
                message.TimeStamp = 0;
                message.Identifier = (uint)MessageId.MinMaxPositionRequestBaseId + (uint)barName;
                message.FrameType = CanMsgFrameType.Data;
                message.DataLength = 8;
                message[0] = (byte)PositionRequest.MoveAllBarsToMinimumPosition;
                message[1] = message[2] = message[3] = message[4] = message[5] = message[6] = message[7] = 0xFF;
                Send(message);
                
                // Auf die Antwort des Controllers Warten
                if (this.eventForMinPositionRequest.WaitOne(1000) == false)
                {
                    operationSuccessful = ReturnType.NotOk;
                }
            }

            return operationSuccessful;
        }

        /// <summary>
        /// Moves all bars to their maximum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public ReturnType MoveAllBarsToMaximumPosition()
        {
            ReturnType operationSuccessful = ReturnType.Ok;
            foreach (Bar barName in Enum.GetValues(typeof(Bar)))
            {
                CanMessage message = new CanMessage();
                message.TimeStamp = 0;
                message.Identifier = (uint)MessageId.MinMaxPositionRequestBaseId + (uint)barName;
                message.FrameType = CanMsgFrameType.Data;
                message.DataLength = 8;
                message[0] = (byte)PositionRequest.MoveAllBarsToMaximumPosition;
                message[1] = message[2] = message[3] = message[4] = message[5] = message[6] = message[7] = 0xFF;
                Send(message);
                
                // Auf die Antwort des Controllers Warten
                if (this.eventForMaxPositionRequest.WaitOne(1000) == false)
                {
                    operationSuccessful = ReturnType.NotOk;
                }
            }

            return operationSuccessful;
        }

        /// <summary>
        /// Sets the bar length in pixel.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="barLengthInPixel">The bar length in pixel.</param>
        public void SetBarLengthInPixel(Bar selectedBar, ushort barLengthInPixel)
        {
            CanMessage message = new CanMessage();
            message.TimeStamp = 0;
            message.Identifier = (uint)MessageId.SetBarLengthInPixelRequestBaseId + (uint)selectedBar;
            message.FrameType = CanMsgFrameType.Data;
            message.DataLength = 8;
            message[0] = (byte)(barLengthInPixel >> 8);
            message[1] = (byte)barLengthInPixel;
            message[2] = 0xFF;
            message[3] = 0xFF;
            message[4] = 0xFF;
            message[5] = 0xFF;
            message[6] = 0xFF;
            message[7] = 0xFF;
            Send(message);
        }

        /// <summary>
        /// Sets the bar angle for zero.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle.</param>
        public void SetBarAngleForZero(Bar selectedBar, int angle)
        {
            CanMessage message = new CanMessage();
            message.TimeStamp = 0;
            message.Identifier = (uint)MessageId.SetAngelForZeroPointRequestBaseId + (uint)selectedBar;
            message.FrameType = CanMsgFrameType.Data;
            message.DataLength = 8;
            message[0] = (byte)(angle >> 8);
            message[1] = (byte)angle;
            message[2] = 0xFF;
            message[3] = 0xFF;
            message[4] = 0xFF;
            message[5] = 0xFF;
            message[6] = 0xFF;
            message[7] = 0xFF;
            Send(message);
        }

        /// <summary>
        /// Sets a bar to a specified angle.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle to set.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetBarToAngle(Bar selectedBar, short angle)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Dispose();
                this.eventForInitStatusRequest.Close();
                this.eventForMaxPositionRequest.Close();
                this.eventForMinPositionRequest.Close();
            }
        }

        /// <summary>
        /// Processes the init status response message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ProcessInitStatusResponseMessage(CanMessage message)
        {
            switch (message[0])
            {
                case (byte)ControllerStatus.Ok:
                    this.currentInitStatus = ControllerStatus.Ok;                    
                    break;
                case (byte)ControllerStatus.Running:
                    this.currentInitStatus = ControllerStatus.Running;
                    break;
                case (byte)ControllerStatus.Error:
                    this.currentInitStatus = ControllerStatus.Error;
                    break;
                default:
                    this.currentInitStatus = ControllerStatus.Error;
                    break;
            }

            this.eventForInitStatusRequest.Set();
        }

        /// <summary>
        /// Processes the min max position response message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ProcessMinMaxPositionResponseMessage(CanMessage message)
        {
            switch (message[0])
            {
                case (byte)PositionRequest.MoveAllBarsToMinimumPosition:
                    if (message[1] == (byte)ControllerStatus.Ok)
                    {
                        this.eventForMinPositionRequest.Set();
                    }

                    break;
                case (byte)PositionRequest.MoveAllBarsToMaximumPosition:
                    if (message[1] == (byte)ControllerStatus.Ok)
                    {
                        this.eventForMaxPositionRequest.Set();
                    }

                    break;
                default:
                    throw new CalibrationException("Unknown Position reached");
            }
        }

        /// <summary>
        /// Verarbeitet ankommende Nachrichten
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanMessageReceivedEventArgs"/> instance containing the event data.</param>
        private void CanAbstractionForCalibrationCanMessageReceived(object sender, CanMessageReceivedEventArgs e)
        {
            switch (e.Message.Identifier & 0xFFF0)
            {
                case (uint)MessageId.MinMaxPositionResponseBaseId:
                    this.ProcessMinMaxPositionResponseMessage(e.Message);
                    break;
                case (uint)MessageId.InitStatusResponseBaseId:
                    this.ProcessInitStatusResponseMessage(e.Message);
                    break;
                default:
                    break;
            }
        }
    }
}
#endif