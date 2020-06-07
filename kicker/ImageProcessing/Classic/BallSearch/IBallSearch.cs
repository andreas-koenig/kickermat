using System;
using System.Collections.Generic;
using System.Text;
using VideoSource;

namespace ImageProcessing
{
    public interface IBallSearch : IVideoSource, IVideoConsumer
    {
        void Start();
    }
}
