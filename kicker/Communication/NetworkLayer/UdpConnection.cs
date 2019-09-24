﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Configuration;
using Microsoft.Extensions.Logging;
using Communication.NetworkLayer.Packets.Udp;
using Communication.NetworkLayer.Packets.Udp.Enums;
using Communication.NetworkLayer.Settings;

namespace Communication.NetworkLayer
{
    public class UdpConnection : UdpClient
    {
        private readonly ILogger<UdpConnection> _logger;
        private IWritableOptions<UdpConnectionSettings> _udpConnectionOptions;

        /// <summary>
        /// Endpoint which is used to recieve data from the gateway.
        /// </summary>
        private IPEndPoint _endPoint;

        /// <summary>
        /// Sequence number for positioning, is incremented for each transmission 
        /// to get information about the order of the received packets at the gateway.
        /// </summary>
        private uint _sequenceNumber;


        public UdpConnection(ILogger<UdpConnection> logger, IWritableOptions<UdpConnectionSettings> udpConnectionOptions)
        {
            _logger = logger;
            _udpConnectionOptions =  udpConnectionOptions;
            _sequenceNumber = 0;
            _endPoint.Address = _udpConnectionOptions.Value.IpAddress;
            _endPoint.Port = _udpConnectionOptions.Value.Port;
        }

        /// <summary>
        /// Sends a <see cref="PlayerPosition"/> to the server.
        /// </summary>
        /// <param name="networkObject">The <see cref="PlayerPosition "/> to send.</param>
        public int Send(PlayerPosition networkObject)
        {
            networkObject.SequenceNumber = this._sequenceNumber;
            byte[] datagram = networkObject.Serialize();
            var ret = this.Send(datagram);
            this._sequenceNumber++;
            return ret;
        }

        /// <summary>
        /// Sends a byte[] to the server.
        /// </summary>
        /// <param name="datagram">The datagram to send.</param>
        public int Send(byte[] datagram)
        {
            return this.Send(datagram, datagram.Length);
        }

        /// <summary>
        /// Reads a datagram from the network stream.
        /// </summary>
        /// <returns>The byte[] read from the stream.</returns>
        internal byte[] Read()
        {
            //Workaround for readTimeout because of threading issues.
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            while (sw.ElapsedMilliseconds < 1000)
            {
                if (this.Client.Available > 0)
                {
                    //byte[] datagram = this.udpClient.ReceiveAsync().Result.Buffer;
                    byte[] datagram = this.Receive(ref this._endPoint);

                    return datagram;
                }
            }
            return new byte[] { };
        }

        public ControllerStatus InitStatus
        {
            get
            {
                Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.CalibrationStatus), 0, this.datagram, 0, 2);
                ZeroFillDatagramFromOffset(2);

                this.Send(this.datagram);

                // TODO: error handling
                byte[] retVal = this.Read();
                return (ControllerStatus)BitConverter.ToUInt16(retVal, 2);
            }
        }

        /// <summary>
        /// Fills the datadatagram with zeroes from the offset to the end.
        /// </summary>
        /// <param name="offset">The offset.</param>
        private void ZeroFillDatagramFromOffset(int offset)
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
