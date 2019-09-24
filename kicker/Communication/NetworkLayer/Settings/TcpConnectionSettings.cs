using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Communication.NetworkLayer.Settings
{
    public class TcpConnectionSettings
    {
        //TODO: Determine correct settings
        public IPAddress IpAddress { get; set; } = System.Net.IPAddress.Parse("127.0.0.1");
        public int TcpPort { get; set; } = 8080;
        public int ReceiveBufferSize { get; set; } = 64;
        public int SendBufferSize { get; set; } = 64;
        public bool NoDelay { get; set; } = true;
    }
}
