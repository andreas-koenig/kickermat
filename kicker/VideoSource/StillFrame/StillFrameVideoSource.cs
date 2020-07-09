using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VideoSource.StillFrame
{
    public class StillFrameVideoSource : BaseVideoSource
    {
        public StillFrameVideoSource(ILogger logger)
            : base(logger) { }

        public override IEnumerable<Channel> GetChannels()
        {
            return new Channel[]
            {
                new Channel("still-frame", "Still Frame", "This channel shows a still frame of the kickermat"),
            };
        }
    }
}
