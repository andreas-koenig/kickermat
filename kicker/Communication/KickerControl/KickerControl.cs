using System;
using System.Collections.Generic;
using System.Text;
using Communication.NetworkLayer;
using Communication.NetworkLayer.Packets.Udp;
using Communication.NetworkLayer.Settings;
using Configuration;
using GameProperties;
using Microsoft.Extensions.Logging;

namespace Communication.KickerControl
{
    class KickerControl : IKickerControl
    {
        private ILogger<IKickerControl> _logger;

        private IWritableOptions<KickerControlSettings> kickerControlOptions;

        private TcpConnection _tcpConnection;

        private UdpConnection _udpConnection;

        public KickerControl(ILogger<KickerControl> logger,
            ILogger<TcpConnection> tcpLogger,
            ILogger<UdpConnection> udpLogger,
            IWritableOptions<KickerControlSettings> kickerControlOptions,
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
