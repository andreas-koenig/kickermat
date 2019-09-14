namespace Communication.NetworkLayer.Udp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using Packets.Tcp.Enum;
    using Packets.Udp;

    /// <summary>
    /// The networklayer service
    /// </summary>
    public sealed class NetworkLayer : IDisposable
    {
        /// <summary>
        /// UDP client for communication with the gateway.
        /// </summary>
        private readonly UdpConnection udpConnection;

        /// <summary>
        /// TCP client for communication with the gateway.
        /// </summary>
        private TcpConnection tcpConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkLayer"/> class.
        /// </summary>
        public NetworkLayer()
        {
            //TODO: Read Settings from config ?
            this.udpConnection = new UdpConnection(System.Net.IPAddress.Parse("127.0.0.1"), 80);
            this.tcpConnection = new TcpConnection(System.Net.IPAddress.Parse("127.0.0.1"), 80);
        }

        public void Connect()
        {
            //TODO: Try Catch ?
            this.tcpConnection.Connect();
        }

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        public void Disconnect()
        {
            this.tcpConnection.Close();
            this.udpConnection.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.udpConnection != null)
            {
                this.udpConnection.Close();
            }

            if (this.tcpConnection != null)
            {
                this.tcpConnection.Close();
            }

            //TODO: Why?
            GC.SuppressFinalize(this);
        }
    }
    
    public class TcpConnection : TcpClient
    {
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

        public TcpConnection(IPAddress ipAddress, int tcpPort)
        {
            //TODO: Load from settings
            this.NoDelay = true;
            this.ReceiveBufferSize = 64;
            this.SendBufferSize = 64;
            this.endPoint.Address = ipAddress;
            this.endPoint.Port = tcpPort;

        }

        //TODO: Use default implementation
        public void Connect()
        {
            //TODO: Try Catch
            //TODO: If there is already a connection ... close and reconnect ?
            this.Connect(endPoint.Address, endPoint.Port);
        }

        public void Close()
        {
            this.tcpReader.Abort();
            this.GetStream().Close();
        }

        private void Read()
        {
            this.tcpReader = new Thread(this.ReadTcpStream);
            this.tcpReader.Name = "TCP Connection";
            this.tcpReader.IsBackground = true;
            this.tcpReader.Start();
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

    public class UdpConnection : UdpClient
    {
        /// <summary>
        /// Endpoint which is used to recieve data from the gateway.
        /// </summary>
        private IPEndPoint endPoint;
        /// <summary>
        /// Sequence number for positioning, is incremented for each transmission 
        /// to get information about the order of the received packets at the gateway.
        /// </summary>
        private uint sequenceNumber;

        public UdpConnection(IPAddress ipAddress, int port)
        {
            this.sequenceNumber = 0;
            this.endPoint.Address = ipAddress;
            this.endPoint.Port = port;
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
        /// Sends a byte[] to the server.
        /// </summary>
        /// <param name="datagram">The datagram to send.</param>
        internal int Send(byte[] datagram)
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
                    byte[] datagram = this.Receive(ref this.endPoint);

                    return datagram;
                }
            }
            return new byte[] { };
        }
    }
}
