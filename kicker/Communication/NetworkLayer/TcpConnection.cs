using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Configuration;
using Communication.NetworkLayer.Packets.Tcp.Enum;
using Communication.NetworkLayer.Settings;
using Microsoft.Extensions.Logging;

namespace Communication.NetworkLayer
{
    public class TcpConnection : TcpClient
    {
        private ILogger<TcpConnection> _logger;
        private IWritableOptions<TcpConnectionSettings> _tcpConnectionOptions;

        /// <summary>
        /// Endpoint which is used to recieve data from the gateway.
        /// </summary>
        private IPEndPoint endPoint;
        /// <summary>
        /// Thread for waiting for incomming messages from the gateway.
        /// </summary>
        private Thread tcpReader;

        /// <summary>
        /// Delegate for handling <see cref="TcpNetworkPacketReceived"/>
        /// </summary>
        /// <param name="packetType">See <see cref="TcpPacketType"/></param>
        /// <param name="data">The received data.</param>
        internal delegate void TcpNetworkPacketReceivedEventHandler(TcpPacketType packetType, object data);

        /// <summary>
        /// Event gets thrown when a network packet was received.
        /// </summary>
        internal event TcpNetworkPacketReceivedEventHandler TcpNetworkPacketReceived;

        public TcpConnection(ILogger<TcpConnection> logger, IWritableOptions<TcpConnectionSettings> tcpConnectionOptions)
        {
            _logger = logger;
            _tcpConnectionOptions = tcpConnectionOptions;
            this.NoDelay = _tcpConnectionOptions.Value.NoDelay;
            this.ReceiveBufferSize = _tcpConnectionOptions.Value.ReceiveBufferSize;
            this.SendBufferSize = _tcpConnectionOptions.Value.SendBufferSize;
            this.endPoint = new IPEndPoint(_tcpConnectionOptions.Value.IpAddress, _tcpConnectionOptions.Value.TcpPort);
        }

        public void Connect()
        {
            try
            {
                this.Connect(endPoint.Address, endPoint.Port);
            }
            catch (Exception e)
            {
                //TODO: Exception Handling Concept;
            }
        }

        public new void Close()
        {
            try
            {
                this.tcpReader.Abort();
                this.GetStream().Close();
            }
            catch (Exception e)
            {
                //TODO: Exception Handling Concept;
            }
        }

        private void Read()
        {
            try
            {
                this.tcpReader = new Thread(this.ReadTcpStream);
                this.tcpReader.Name = "TCP Connection";
                this.tcpReader.IsBackground = true;
                this.tcpReader.Start();
            }
            catch (Exception e)
            {
                //TODO: Exception Handling Concept;
            }
        }

        /// <summary>
        /// TCP connection thread. Reading the TCP packets and initializing connection. (Service connection.)
        /// </summary>
        private void ReadTcpStream()
        {
            using (NetworkStream networkStream = this.GetStream())
            {
                try
                {
                    //TODO: Establish Connection?
                    byte[] buffer = new byte[6];
                    Buffer.BlockCopy(((IPEndPoint)this.Client.LocalEndPoint).Address.GetAddressBytes(), 0, buffer, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(((IPEndPoint)this.Client.LocalEndPoint).Port), 0, buffer, 4, 2);
                    networkStream.Write(buffer, 0, buffer.Length);

                    byte[] header = new byte[4];
                    while (this.Connected)
                    {
                        // Read header
                        networkStream.Read(header, 0, 4);
                        TcpPacketType packetType = (TcpPacketType)BitConverter.ToUInt16(header, 0);
                        ushort packetLength = BitConverter.ToUInt16(header, 2);

                        // Read as much data as specified in header
                        byte[] data = new byte[packetLength];
                        networkStream.Read(data, 0, packetLength);

                        object dataObj = null;

                        switch (packetType)
                        {
                            case TcpPacketType.Logging:
                                dataObj = Encoding.ASCII.GetString(data);
                                break;

                            default:
                                packetType = TcpPacketType.UnknownPacketType;
                                dataObj = null;
                                break;
                        }

                        if (this.TcpNetworkPacketReceived != null)
                        {
                            this.TcpNetworkPacketReceived(packetType, dataObj);
                        }
                    }
                }
                catch (IOException e)
                {
                    //SwissKnife.ShowException(this, e);
                }
            }
        }
    }
}
