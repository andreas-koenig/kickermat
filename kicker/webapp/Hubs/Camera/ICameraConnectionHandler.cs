using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Webapp.Hubs
{
    public interface ICameraConnectionHandler
    {
        void StartStream(VideoInput videoInput, string clientId, Channel<string> channel,
            CancellationToken cancellationToken);
        void AbortConnection(string clientId);
    }
}
