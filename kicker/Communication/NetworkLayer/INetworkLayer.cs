using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Communication.NetworkLayer
{
    public interface INetworkLayer
    {
        void Connect(IPAddress ipAddress, ushort udpPort, ushort tcpPort);

        void Disconnect();
    }
}
