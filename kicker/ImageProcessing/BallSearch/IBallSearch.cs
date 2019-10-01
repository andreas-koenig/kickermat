using System;
using System.Collections.Generic;
using System.Text;
using VideoSource;

namespace ImageProcessing.BallSearch
{
    public interface IBallSearch : IVideoSource, IVideoConsumer
    {
        void Start();
    }
}
