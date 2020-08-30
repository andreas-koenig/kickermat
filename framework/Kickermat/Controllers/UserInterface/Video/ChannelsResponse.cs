using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.UserInterface.Video;

namespace Kickermat.Controllers.UserInterface.Video
{
    public class ChannelsResponse
    {
        public ChannelsResponse(IEnumerable<IVideoChannel> channels, IVideoChannel currentChannel)
        {
            Channels = channels;
            CurrentChannel = currentChannel;
        }

        public IEnumerable<IVideoChannel> Channels { get; set; }

        public IVideoChannel CurrentChannel { get; set; }
    }
}

