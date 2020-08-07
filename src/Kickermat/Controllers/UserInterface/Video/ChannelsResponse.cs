using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.UserInterface.Video;

namespace Webapp.Controllers.UserInterface.Video
{
    public class ChannelsResponse
    {
        public IEnumerable<IVideoChannel> Channels { get; set; }

        public IVideoChannel CurrentChannel { get; set; }

        public ChannelsResponse(IEnumerable<IVideoChannel> channels, IVideoChannel currentChannel)
        {
            Channels = channels;
            CurrentChannel = currentChannel;
        }
    }
}
