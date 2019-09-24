namespace Communication.Calibration.UdpGateway
{
    using System;
    using Communication.NetworkLayer.Packets.Udp;
    using GameProperties;
    using NetworkLayer;
    using NetworkLayer.Packets.Udp.Enums;
    using NetworkLayer.Udp;


    /// <summary>
    /// Implementation of calibration call for the motors via controller with UDP. The calibration is needed for the ImageProcessing
    /// </summary>
    public class Calibration : ICalibrationControl
    {

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
        public void SetAllAnglesAndPositionsToZero()
        {
            PlayerPosition positionNetworkObject = new PlayerPosition();
            positionNetworkObject.OptionsValidFor = PositionBits.All;
            positionNetworkObject.ReplyRequested = PositionBits.None;
            //Send the networkObject several times because for whatever Reasons sometimes The Angles are not being set on the first try!
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
            this.networkLayer.Send(positionNetworkObject);
        }
    }
}
