using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    public interface IVideoProcessor : IVideoSource, IVideoConsumer
    {
    }
}
