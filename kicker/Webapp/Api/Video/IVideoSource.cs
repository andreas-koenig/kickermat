using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Webapp.Api.Video
{
    public interface IVideoSource<T> : IObservable<T>
    {
        public IEnumerable<VideoChannel> Channels { get; }

        public VideoChannel Channel { get; set; }
    }
}
