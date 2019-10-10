using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Communication.NetworkLayer.Packets.Tcp.Enum;
using Communication.NetworkLayer.Settings;
using Configuration;
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
        private IPEndPoint _endPoint;

        /// <summary>
        /// Thread for waiting for incomming messages from the gateway.
        /// </summary>
        private Thread _tcpReader;

        /// <summary>
        /// Delegate for handling <see cref="TcpNetworkPacketReceived"/>.
        /// </summary>
        /// <param name="packetType">See <see cref="TcpPacketType"/>.</param>
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
            NoDelay = _tcpConnectionOptions.Value.NoDelay;
            ReceiveBufferSize = _tcpConnectionOptions.Value.ReceiveBufferSize;
            SendBufferSize = _tcpConnectionOptions.Value.SendBufferSize;
            _endPoint = new IPEndPoint(_tcpConnectionOptions.Value.IpAddress, _tcpConnectionOptions.Value.TcpPort);
        }

        public void Connect()
        {
            try
            {
                Connect(_endPoint.Address, _endPoint.Port);
            }
            catch (Exception ex)
            {
                // TODO: Exception Handling Concept;
                _logger.LogError("Failed to connect to gateway via TCP", ex);
            }
        }

        public new void Close()
        {
            try
            {
                _tcpReader.Abort();
                GetStream().Close();
            }
            catch (Exception ex)
            {
                // TODO: Exception Handling Concept;
                _logger.LogError("Failed to close connection to gateway (TCP)", ex);
            }
        }

        private void Communicate()
        {
            try
            {
                _tcpReader = new Thread(ReadWriteTcpStream);
                _tcpReader.Name = "TCP Connection";
                _tcpReader.IsBackground = true;
                _tcpReader.Start();
            }
            catch (Exception ex)
            {
                // TODO: Exception Handling Concept;
                _logger.LogError("Failed to start communication with gateway via TPC", ex);
            }
        }

        /// <summary>
        /// TCP connection thread. Reading the TCP packets and initializing connection. (Service connection.)
        /// </summary>
        private void ReadWriteTcpStream()
        {
            using (NetworkStream networkStream = GetStream())
            {
                try
                {
                    // TODO: Establish Connection?
                    byte[] buffer = new byte[6];
                    Buffer.BlockCopy(((IPEndPoint)Client.LocalEndPoint).Address.GetAddressBytes(), 0, buffer, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(((IPEndPoint)Client.LocalEndPoint).Port), 0, buffer, 4, 2);
                    networkStream.Write(buffer, 0, buffer.Length);

                    byte[] header = new byte[4];
                    while (Connected)
                    {
                        // TODO: Errorhandling, conversion might fail
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

                        if (TcpNetworkPacketReceived != null)
                        {
                            TcpNetworkPacketReceived(packetType, dataObj);
                        }
                    }
                }
                catch (IOException ex)
                {
                    // TODO: Exception Handling Concept;
                    _logger.LogError("Failed to read from or write to TCP connection", ex);
                }
            }
        }
    }
}
