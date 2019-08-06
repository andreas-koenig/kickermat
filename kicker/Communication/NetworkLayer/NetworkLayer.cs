namespace Communication.NetworkLayer.Udp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    //using System.Windows.Forms;
    using Packets.Tcp.Enum;
    using Packets.Udp;
    //using Utilities;

    /// <summary>
    /// The netwoklayer service
    /// </summary>
    public sealed class NetworkLayer : IDisposable
    {
        /// <summary>
        /// UDP client for communication with the gateway.
        /// </summary>
        private readonly UdpClient udpClient;

        /// <summary>
        /// TCP client for communication with the gateway.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// Endpoint which is used to recieve data from the gateway.
        /// </summary>
        private IPEndPoint endPoint;

        /// <summary>
        /// Sequence number for positioning, is incremented for each transmission 
        /// to get information about the order of the received packets at the gateway.
        /// </summary>
        private uint sequenceNumber;

        /// <summary>
        /// Thread for waiting for incomming messages from the gateway.
        /// </summary>
        private Thread tcpReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkLayer"/> class.
        /// </summary>
        public NetworkLayer()
        {
            this.sequenceNumber = 0;
            this.udpClient = new UdpClient();
        }

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

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <param name="ipAddress">Remote hosts ip address.</param>
        /// <param name="udpPort">The udp port to use.</param>
        /// <param name="tcpPort">The tcp port to use.</param>
        public void Connect(IPAddress ipAddress, ushort udpPort, ushort tcpPort)
        {
            this.udpClient.Connect(ipAddress, udpPort);
            this.endPoint = (IPEndPoint)this.udpClient.Client.RemoteEndPoint;

            this.tcpClient = new TcpClient();
            this.tcpClient.NoDelay = true;
            this.tcpClient.ReceiveBufferSize = 64;
            this.tcpClient.SendBufferSize = 64;
            this.tcpClient.Connect(ipAddress, tcpPort);

            this.tcpReader = new Thread(this.TcpConnection);
            this.tcpReader.Name = "TCP Connection";
            this.tcpReader.IsBackground = true;
            this.tcpReader.Start();
        }

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        public void Disconnect()
        {
            this.tcpReader.Abort();
            this.tcpClient.GetStream().Close();
            this.udpClient.Close();
        }

        /// <summary>
        /// Sends a <see cref="PlayerPositions"/> to the server.
        /// </summary>
        /// <param name="networkObject">The <see cref="PlayerPositions "/> to send.</param>
        public int Send(PlayerPositions networkObject)
        {
            networkObject.SequenceNumber = this.sequenceNumber;
            byte[] datagram = networkObject.Serialize();
            var ret = this.Send(datagram);
            this.sequenceNumber++;
            return ret;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.udpClient != null)
            {
                this.udpClient.Close();
            }

            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sends a byte[] to the server.
        /// </summary>
        /// <param name="datagram">The datagram to send.</param>
        internal int Send(byte[] datagram)
        {
            return this.udpClient.Send(datagram, datagram.Length);
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
                if (udpClient.Client.Available > 0)
                {
                    //byte[] datagram = this.udpClient.ReceiveAsync().Result.Buffer;
                    byte[] datagram = this.udpClient.Receive(ref this.endPoint);

                    return datagram;
                }
            }
            return new byte[] { };
        }

        /// <summary>
        /// TCP connection thread. Reading the TCP packets and initializing connection. (Service connection.)
        /// </summary>
        private void TcpConnection()
        {
            using (NetworkStream networkStream = this.tcpClient.GetStream())
            {
                try
                {
                    byte[] buffer = new byte[6];
                    Buffer.BlockCopy(((IPEndPoint)this.udpClient.Client.LocalEndPoint).Address.GetAddressBytes(), 0, buffer, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(((IPEndPoint)this.udpClient.Client.LocalEndPoint).Port), 0, buffer, 4, 2);
                    networkStream.Write(buffer, 0, buffer.Length);

                    byte[] header = new byte[4];
                    while (this.tcpClient.Connected)
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