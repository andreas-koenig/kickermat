namespace Communication.Calibration.UdpGateway
{
    using System;
    using Communication.NetworkLayer.Packets.Udp;
    using GlobalDataTypes;
    using NetworkLayer;
    using NetworkLayer.Packets.Udp.Enums;
    using NetworkLayer.Udp;
    using PluginSystem;
    using Utilities;

    /// <summary>
    /// Implementation of calibration call to the controller via UDP.
    /// </summary>
    public class UdpCalibration : ICalibrationControl
    {
        /// <summary>
        /// Ask Stefan Seifert.
        /// </summary>
        private readonly UdpNetworkLayer networkLayer;

        /// <summary>
        /// Ask Stefan Seifert.
        /// </summary>
        private readonly byte[] datagram;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpCalibration"/> class.
        /// </summary>
        public UdpCalibration()
        {
            this.datagram = new byte[Constants.DatagramLength];

            this.networkLayer = ServiceLocator.LocateService<UdpNetworkLayer>();
            if (this.networkLayer == null)
            {
                SwissKnife.ShowError(this, "No network service available.");
            }
        }

        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        public ControllerStatus InitStatus
        {
            get
            {
                Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.CalibrationStatus), 0, this.datagram, 0, 2);
                FillDatadatagram(this.datagram, 2);

                this.networkLayer.Send(this.datagram);

                // TODO: error handling
                byte[] retVal = this.networkLayer.Read();
                return (ControllerStatus)BitConverter.ToUInt16(retVal, 2);
            }
        }

        /// <summary>
        /// Moves all bars to their maximum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public ReturnType MoveAllBarsToMaximumPosition()
        {
            ReturnType returnType = ReturnType.Ok;

            foreach (Bar barName in Enum.GetValues(typeof(Bar)))
            {
                if (barName != Bar.All)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetMaxPosition), 0, this.datagram, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)barName), 0, this.datagram, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)255), 0, this.datagram, 4, 2);
                    FillDatadatagram(this.datagram, 6);

                    this.networkLayer.Send(this.datagram);
                    byte[] returnDatagram = this.networkLayer.Read();

                    if ((ControllerStatus)BitConverter.ToUInt16(returnDatagram, 2) != ControllerStatus.Ok)
                    {
                        returnType = ReturnType.NotOk;
                    }
                }
            }

            return returnType;
        }

        /// <summary>
        /// Moves all bars to their minimum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public ReturnType MoveAllBarsToMinimumPosition()
        {
            ReturnType returnType = ReturnType.Ok;

            foreach (Bar barName in Enum.GetValues(typeof(Bar)))
            {
                if (barName != Bar.All)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetMinPosition), 0, this.datagram, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)barName), 0, this.datagram, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)0), 0, this.datagram, 4, 2);
                    FillDatadatagram(this.datagram, 6);

                    this.networkLayer.Send(this.datagram);

                    byte[] returnDatagram = this.networkLayer.Read();

                    if ((ControllerStatus)BitConverter.ToUInt16(returnDatagram, 2) != ControllerStatus.Ok)
                    {
                        returnType = ReturnType.NotOk;
                    }
                }
            }

            return returnType;
        }

        /// <summary>
        /// Sets the bar length in pixel.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="barLengthInPixel">The bar length in pixel.</param>
        public void SetBarLengthInPixel(Bar selectedBar, ushort barLengthInPixel)
        {
            ushort udpId = (ushort)selectedBar;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetBarLengthInPixel), 0, this.datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(udpId), 0, this.datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(barLengthInPixel), 0, this.datagram, 4, 2);
            FillDatadatagram(this.datagram, 6);

            this.networkLayer.Send(this.datagram);
        }

        /// <summary>
        /// Sets the bar angle for zero.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle.</param>
        public void SetBarAngleForZero(Bar selectedBar, int angle)
        {
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetNullAngle), 0, this.datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)selectedBar), 0, this.datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)angle), 0, this.datagram, 0, 4);
            FillDatadatagram(this.datagram, 6);

            this.networkLayer.Send(this.datagram);
        }

        /// <summary>
        /// Sets a bar to a specified angle.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle to set.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetAllAnglesAndPositionsToZero()
        {
            PositionNetworkObject positionNetworkObject = new PositionNetworkObject();
            positionNetworkObject.OptionsValidFor = PositionBits.All;
            positionNetworkObject.ReplyRequested = PositionBits.None;
            //Send the networkObject several times because for whatever Reasons sometimes The Angles are not being set on the first try!
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            return ReturnType.Ok;
        }

        /// <summary>
        /// Fills the datadatagram.
        /// </summary>
        /// <param name="datagram">The datagram.</param>
        /// <param name="offset">The offset.</param>
        private static void FillDatadatagram(byte[] datagram, int offset)
        {
            if (offset > datagram.Length)
            {
                throw new ArgumentException("Offset is out of datagram bounds");
            }

            for (int i = offset; i < datagram.Length; i++)
            {
                datagram[i] = 0x00;
            }
        }
    }
}