using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Video;

namespace Webapp.Controllers.UserInterface.Video
{
    public class ChannelsResponse
    {
        public IEnumerable<VideoChannel> Channels { get; set; }

        public VideoChannel CurrentChannel { get; set; }

        public ChannelsResponse(IEnumerable<VideoChannel> channels, VideoChannel currentChannel)
        {
            Channels = channels;
            CurrentChannel = currentChannel;
        }
    }
}
