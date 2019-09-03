namespace Communication.Calibration.UdpGateway
{
    using System;
    using Communication.NetworkLayer.Packets.Udp;
    using Game;
    using NetworkLayer;
    using NetworkLayer.Packets.Udp.Enums;
    using NetworkLayer.Udp;


    /// <summary>
    /// Implementation of calibration call for the motors via controller with UDP. The calibration is needed for the ImageProcessing
    /// </summary>
    public class Calibration : ICalibrationControl
    {
        private readonly NetworkLayer networkLayer;

        /// <summary>
        /// UDP-Datagram
        /// </summary>
        private readonly byte[] datagram;

        /// <summary>
        /// Length of the UDP-datagram
        /// </summary>
        public const int datagramLength = 24;

        /// <summary>
        /// Initializes a new instance of the <see cref="Calibration"/> class.
        /// </summary>
        public Calibration()
        {
            this.datagram = new byte[datagramLength];

            //TODO: Substitute Plugin System
            //this.networkLayer = ServiceLocator.LocateService<NetworkLayer>();

            //TODO: Try-Catch with proper Exception-Handling instead of Swissknife
            //if (this.networkLayer == null)
            //{
            //    SwissKnife.ShowError(this, "No network service available.");
            //}
        }

        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        public ControllerStatus InitStatus
        {
            get
            {
                //TODO: Try-Catch with proper Exception-Handling
                Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.CalibrationStatus), 0, this.datagram, 0, 2);
                this.ZeroFillDatagramFromOffset(2);

                this.networkLayer.Send(this.datagram);

                byte[] retVal = this.networkLayer.Read();
                return (ControllerStatus)BitConverter.ToUInt16(retVal, 2);
            }
        }

        /// <summary>
        /// Moves all bars to their maximum positions.
        /// </summary>
        /// <returns>
        /// void if the operation has been successfully, else an exception
        /// </returns>
        public void MoveAllBarsToMaximumPosition()
        {
            MoveAllBarsToPosition(UdpPacketType.SetMaxPosition);
        }

        /// <summary>
        /// Moves all bars to their minimum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public void MoveAllBarsToMinimumPosition()
        {
            MoveAllBarsToPosition(UdpPacketType.SetMinPosition);
        }

        //TODO: Use PlayerControl for Calibration ?
        private void MoveAllBarsToPosition(UdpPacketType position)
        {
            //TODO: Only SetMinPosition and SetMaxPosition valid for calibration
            foreach (Bar barName in Enum.GetValues(typeof(Bar.BarType)))
            {
                //TODO: Move all bars ?!
                if (barName.Equals(Bar.BarType.All))
                {
                    //TODO: Try-Catch
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)position), 0, this.datagram, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)Bar.BarType.All), 0, this.datagram, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)0), 0, this.datagram, 4, 2);
                    this.ZeroFillDatagramFromOffset(6);

                    this.networkLayer.Send(this.datagram);

                    byte[] returnDatagram = this.networkLayer.Read();

                    //TODO: Throw Exception
                    //if ((ControllerStatus)BitConverter.ToUInt16(returnDatagram, 2) != ControllerStatus.Ok)
                    //{
                    //    returnType = ReturnType.NotOk;
                    //}
                }
            }
        }

        /// <summary>
        /// Sets the bar length in pixel.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="barLengthInPixel">The bar length in pixel.</param>
        public void SetBarLengthInPixel(Bar selectedBar, ushort barLengthInPixel)
        {
            ushort udpId = (ushort)selectedBar.barSelection;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetBarLengthInPixel), 0, this.datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(udpId), 0, this.datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(barLengthInPixel), 0, this.datagram, 4, 2);
            this.ZeroFillDatagramFromOffset(6);

            this.networkLayer.Send(this.datagram);
        }

        /// <summary>
        /// Sets the bar angle for zero.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle.</param>
        public void SetBarAngleForZero(Bar selectedBar, int angle)
        {
            // Removed implementation as this legacy-code was never called
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets a bar to a specified angle.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle to set.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public void SetAllAnglesAndPositionsToZero()
        {
            PlayerPositions positionNetworkObject = new PlayerPositions();
            positionNetworkObject.OptionsValidFor = PositionBits.All;
            positionNetworkObject.ReplyRequested = PositionBits.None;
            //Send the networkObject several times because for whatever Reasons sometimes The Angles are not being set on the first try!
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
        }

        /// <summary>
        /// Fills the datadatagram with zeroes from the offset to the end.
        /// </summary>
        /// <param name="offset">The offset.</param>
        private void ZeroFillDatagramFromOffset( int offset)
        {
            if (offset > this.datagram.Length)
            {
                throw new ArgumentException("Offset is out of datagram bounds");
            }

            for (int i = offset; i < this.datagram.Length; i++)
            {
                this.datagram[i] = 0x00;
            }
        }
    }
}
