using System;
using System.Collections.Generic;
using System.Text;
using Communication.NetworkConnections;
using Communication.NetworkConnections.Packets.Udp;
using Communication.NetworkConnections.Settings;
using Configuration;
using GameProperties;
using Microsoft.Extensions.Logging;

namespace Communication
{
    public class Communication : ICommunication
    {
        private ILogger<ICommunication> _logger;

        private IWritableOptions<CommunicationSettings> _kickerControlOptions;

        private TcpConnection _tcpConnection;

        private UdpConnection _udpConnection;

        public Communication(
            ILogger<Communication> logger,
            ILogger<TcpConnection> tcpLogger,
            ILogger<UdpConnection> udpLogger,
            IWritableOptions<CommunicationSettings> kickerControlOptions,
            IWritableOptions<TcpConnectionSettings> tcpConnectionSettings,
            IWritableOptions<UdpConnectionSettings> udpConnectionSettings)
        {
            _logger = logger;
            _tcpConnection = new TcpConnection(tcpLogger, tcpConnectionSettings);
            _udpConnection = new UdpConnection(udpLogger, udpConnectionSettings);
        }

        public void Send(NetworkObject position)
        {
            _udpConnection.Send(position);

            // TODO: Use await instead of blocking?
            byte[] returnDatagram = _udpConnection.Read();

            // if ((ControllerStatus)BitConverter.ToUInt16(returnDatagram, 2) != ControllerStatus.Ok)
            // Error, CommunicationException?
        }
    }
}
