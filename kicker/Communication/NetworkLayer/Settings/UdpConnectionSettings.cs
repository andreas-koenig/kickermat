using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Communication.NetworkLayer.Settings
{
    public class UdpConnectionSettings
    {
        //TODO: Determine correct settings
        public IPAddress IpAddress { get; set; } = System.Net.IPAddress.Parse("127.0.0.1");
        public int Port { get; set; } = 4545;
    }
}
